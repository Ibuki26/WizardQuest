using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.InputSystem;

public class WizardPresenter : MonoBehaviour
{
    [SerializeField] private int hitPoint;
    [SerializeField] private int strength;
    [SerializeField] private int defense;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private GameObject overWindow;

    private WizardModel _model;
    private WizardView _view;
    private WizardStateController stateCon;
    private GroundChecker ground;
    private MagicCreator[] magics = new MagicCreator[2];
    private PlayerInput _playerInput;
    private Rigidbody2D rb;

    private float gravity = -6; //落下時のy方向の速度
    private float adjustRaycast_y = 0.005f; //Raycastをするときの発生座標の調整用数値
    private float jumpLimit = 0.7f; //ジャンプする限界時間

    public WizardModel Model => _model;
    public MagicCreator[] Magics => magics;

    private void SetM()
    {
        //魔法のセット
        magics[0] = MagicCreator.Initialize(MyStatus.magics[0]);
        magics[1] = MagicCreator.Initialize(MyStatus.magics[1]);
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void ManualStart()
    {
        _model = new WizardModel(hitPoint, strength, defense, speed, jump);
        _view = GetComponent<WizardView>();
        rb = GetComponent<Rigidbody2D>();
        _view.ManualStart();
        stateCon = GetComponent<WizardStateController>();
        stateCon.SetState(WizardState.Standing);
        ground = GetComponent<GroundChecker>();

        //Magicの取得
        SetM();
        //装備の取得
        //UIへの情報の送信　いるか不明
    }

    public void ManualFixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.7f, 0), Color.yellow, 1f);
        //地面の接地判定
        IsGround();
        AdjustVelocity_y();
        rb.velocity = Model.PlayerVelocity;
    }

    //PlayerInputへの関数の登録
    private void OnEnable()
    {
        if (_playerInput == null) return;

        _playerInput.onActionTriggered += OnMove;
        _playerInput.onActionTriggered += OnJump;
        _playerInput.onActionTriggered += OnMagic;
    }

    //PlayerInputへの関数登録の解除
    private void OnDisable()
    {
        if (_playerInput == null) return;

        _playerInput.onActionTriggered -= OnMove;
        _playerInput.onActionTriggered -= OnJump;
        _playerInput.onActionTriggered -= OnMagic;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.name != "Move") return;

        //スティックに触っていないときの処理
        if (context.canceled)
        {
            _view.SetAnimation("isRun", false);
            Model.PlayerVelocityX = 0f;
            return;
        }

        //x軸方向の入力情報の取得
        float xAxis = context.ReadValue<Vector2>().x;

        //進行方向の記録と画像の向き調整
        _model.Direction = (xAxis > 0) ? 1 : -1;
        transform.localScale = new Vector3(0.35f * _model.Direction, 0.35f, 1);

        _view.SetAnimation("isRun", true);
        Model.PlayerVelocityX = Model.RunSpeed(Model.Speed, Model.Direction);
    }

    public async void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed
            || context.action.name != "Jump") return;

        if (stateCon.HasState(WizardState.Standing))
        {
            //ジャンプを始める処理
            AudioManager.Instance.PlaySE(AudioType.jump);
            _view.SetAnimation("isJump", true);
            Model.PlayerVelocityY = _model.Jump;
            stateCon.AddState(WizardState.Jumping);

            await UniTask.Delay(TimeSpan.FromSeconds(jumpLimit), cancellationToken: this.GetCancellationTokenOnDestroy());
            //落ちる処理
            _view.SetAnimation("isJump", false);
            stateCon.DeleteState(WizardState.Jumping);
            stateCon.AddState(WizardState.falling);
        }
    }

    private async void OnMagic(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed
            || (context.action.name != "Magic1" && context.action.name != "Magic2")) return;
        var magicNum = (context.action.name == "Magic1") ? 0 : 1; //魔法1か2か判定

        //魔法実行中とクールタイム中のときは実行しない
        if (stateCon.HasState(WizardState.Magicing) || magics[magicNum].IsCoolTime) return;

        stateCon.AddState(WizardState.Magicing);
        _view.SetAnimTrigger("attack");

        //アニメーションのタイミング調整
        await UniTask.WaitUntil(() =>
            _view.StateInfo.IsName("Attack") &&
            _view.StateInfo.normalizedTime >= 0.9f
        , cancellationToken: this.GetCancellationTokenOnDestroy());

        magics[magicNum].CreateMagic(this, magicNum);
        //別のボタンの魔法をほぼ同時に使えないように待機
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        stateCon.DeleteState(WizardState.Magicing);
    }

    public async UniTask DamageFromEnemy(int enemyAttack, int enemyStrength)
    {
        //ダメージ無視状態中と魔法使用中はダメージを受けない
        if (stateCon.HasState(WizardState.IgnoreDamage)
            || stateCon.HasState(WizardState.Magicing)) return;
        if (_model.HitPoint.Value == 0) return;

        //無敵状態の開始
        stateCon.AddState(WizardState.IgnoreDamage);

        //ダメージ量の計算
        var damage = Model.CalculateDamage(enemyAttack, enemyStrength);
        //体力-ダメージ量が負の値になったら0、そうでないなら体力-ダメージ量をそのまま代入
        _model.HitPoint.Value = _model.DecreaseHitPoint(damage);
        Die();

        //ダメージ演出
        AudioManager.Instance.PlaySE(AudioType.damage_player);
        _view.SetAnimTrigger("hurt");

        //無敵状態の待機
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
        //無敵状態の解除
        stateCon.DeleteState(WizardState.IgnoreDamage);
    }

    public async UniTask DamageFromGimmick(int damage)
    {
        //ダメージ無視状態中と魔法使用中はダメージを受けない
        if (stateCon.HasState(WizardState.IgnoreDamage)
            || stateCon.HasState(WizardState.Magicing)) return;
        if (_model.HitPoint.Value == 0) return;

        //無敵状態の開始
        stateCon.AddState(WizardState.IgnoreDamage);

        _model.HitPoint.Value = _model.DecreaseHitPoint(damage);
        Die();

        //ダメージ演出
        AudioManager.Instance.PlaySE(AudioType.damage_player);
        _view.SetAnimTrigger("hurt");

        //無敵状態の待機
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
        //無敵状態の解除
        stateCon.DeleteState(WizardState.IgnoreDamage);
    }

    public void Heal(int healPoint)
    {
        //今の体力と回復値を足して、上限値を超えたら上限値を代入、そうでないなら体力+回復値を代入
        _model.HitPoint.Value = Model.IncreaseHitPoint(healPoint);
    }

    private void Die()
    {
        if (_model.HitPoint.Value > 0) return;
        //PlayerInputへの関数登録の解除
        _playerInput.onActionTriggered -= OnMove;
        _playerInput.onActionTriggered -= OnJump;
        _playerInput.onActionTriggered -= OnMagic;

        AudioManager.Instance.PlaySE(AudioType.gameOver);
        Model.PlayerVelocity = Vector2.zero;
        _view.SetAnimTrigger("die");
        //ゲームオーバー画面の表示
        _playerInput.actions.Disable();
        _playerInput.actions.FindActionMap("UI");
        overWindow.SetActive(true);
    }

    //地面と接しているかの判定
    private void IsGround()
    {
        //地面と接していたらStandingを追加
        if (ground.IsGround(adjustRaycast_y))
            stateCon.AddState(WizardState.Standing);
        //地面から離れていたらStandingを削除
        else
            stateCon.DeleteState(WizardState.Standing);
    }

    //ジャンプ中以外でのy方向の速度の調整
    private void AdjustVelocity_y()
    {
        //地面と接しているときにy方向の速度が負の値のときの処理
        if (rb.velocity.y < 0 && stateCon.HasState(WizardState.Standing))
        {
            Model.PlayerVelocityY = 0f;
            stateCon.DeleteState(WizardState.falling);
        }

        //ジャンプ中以外で空中にいるときの処理
        if (!stateCon.HasState(WizardState.Jumping) &&
            !stateCon.HasState(WizardState.Standing))
        {
            Model.PlayerVelocityY = gravity;
        }
    }
}
