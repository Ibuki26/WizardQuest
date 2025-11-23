using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.InputSystem;
using System.Linq;

public class WizardPresenter : MonoBehaviour
{
    //初期値の登録
    [SerializeField] private WizardModelData data;
    [SerializeField] private BuffEffecter buffEffecter;

    static private WizardPresenter s_Instance;

    static public WizardPresenter Instance { get { return s_Instance; } }

    private WizardModel _model;
    private WizardView _view;
    private FlagsController<WizardState> stateCon;
    private MagicCreator[] magics = new MagicCreator[2];
    private PlayerInput _playerInput;
    private Rigidbody2D rb;

    private WizardMovement movement;
    private CharacterRaycaster raycaster;
    private ConditionManager conditionManager;

    public WizardModel Model => _model;
    public MagicCreator[] Magics => magics;
    public FlagsController<WizardState> StateCon => stateCon;

    public ConditionManager ConditionManager => conditionManager;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void ManualStart()
    {
        s_Instance = this;
        _model = new WizardModel(data);
        _view = GetComponent<WizardView>();
        rb = GetComponent<Rigidbody2D>();
        _view.ManualStart();
        stateCon = new FlagsController<WizardState>();
        movement = GetComponent<WizardMovement>();
        movement.Initialize(_model, stateCon);
        raycaster = GetComponent<CharacterRaycaster>();
        raycaster.ManualStart();
        conditionManager = GetComponent<ConditionManager>();
        conditionManager.ManualStart();

        //Magicの取得
        for(int i = 0; i < 2; i++)
        {
            magics[i] = MagicCreator.Initialize(MyStatusManager.Instance.FetchMagic()[i], Instantiate);
        }

        //装備のゲーム開始スキル起動
        buffEffecter.ManualStart();
        foreach (var buff in magics.OfType<BuffMagicCreator>())
        {
            //BuffEffecterクラスへAnimatorの登録
            buff.Initialize(buffEffecter);
        }
        //SkillManager.Instance.TriggerOnGameStart(magics, conditionManager);

        //装備のステータスの適応
        foreach (var equipment in MyStatusManager.Instance.FetchEquipment())
        {
            _model.AddEquipmentStatus(equipment);
        }
    }

    public void ManualFixedUpdate()
    {
        //地面の接地判定
        raycaster.ManualFixedUpdate();
        IsGround();
        IsCeiling();
        _model.PlayerVelocity = movement.UpdateVelocity(_model.PlayerVelocity);
        rb.velocity = _model.PlayerVelocity;
    }

    //PlayerInputへの関数の登録
    private void OnEnable()
    {
        if (_playerInput == null) return;

        _playerInput.onActionTriggered += OnMove;
        _playerInput.onActionTriggered += OnJump;
        _playerInput.onActionTriggered += OnMagic;
        _playerInput.onActionTriggered += OnOption;
    }

    //PlayerInputへの関数登録の解除
    private void OnDisable()
    {
        if (_playerInput == null) return;

        _playerInput.onActionTriggered -= OnMove;
        _playerInput.onActionTriggered -= OnJump;
        _playerInput.onActionTriggered -= OnMagic;
        _playerInput.onActionTriggered -= OnOption;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.name != "Move") return;

        //x軸方向の入力情報の取得
        float xAxis = context.ReadValue<Vector2>().x;
        movement.XAxis = xAxis;

        //スティックに触っていないときの処理
        if (Mathf.Abs(xAxis) < 0.1f)
        {
            _view.SetAnimation("isRun", false);
            return;
        }

        //進行方向の記録と画像の向き調整
        _model.Direction = (xAxis > 0) ? 1 : -1;
        _view.FlipX(_model.Direction);

        //方向転換回数の記録
        PlayDataRecorder.Instance.CheckTurn(_model.Direction);

        //BuffEffecterの向きの調整
        foreach (var buff in magics.OfType<BuffMagicCreator>())
        {
            buff.Status.Magic.SetBuffEffecterSpriteFlip(_model.Direction);
        }

        PlayDataRecorder.Instance.AddMoveLength(_model.GetRunSpeed()*Time.fixedDeltaTime);
        
        _view.SetAnimation("isRun", true);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.action.name != "Jump") return;

        if (stateCon.HasState(WizardState.Standing) && context.performed)
        {
            PlayDataRecorder.Instance.AddJump();

            //ジャンプを始める処理
            AudioManager.Instance.PlaySE(AudioType.jump);
            _view.SetAnimation("isJump", true);
            _model.PlayerVelocityY = _model.Jump;
            stateCon.AddState(WizardState.Jumping);
            movement.PushJumpButton = true;
        }
        if (context.canceled && stateCon.HasState(WizardState.Jumping))
        {
            movement.PushJumpButton = false;
            _view.SetAnimation("isJump", false);
            stateCon.DeleteState(WizardState.Jumping);
        }
    }

    private async void OnMagic(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed
            || (context.action.name != "Magic1" && context.action.name != "Magic2")) return;

        var magicNum = (context.action.name == "Magic1") ? 0 : 1; //魔法1か2か判定

        //魔法実行中とクールタイム中のときは実行しない
        if (stateCon.HasState(WizardState.Magicing) || magics[magicNum].IsCoolTime) return;

        PlayDataRecorder.Instance.AddCastCount(magicNum);

        stateCon.AddState(WizardState.Magicing);
        _view.SetAnimTrigger("attack");

        //アニメーションのタイミング調整
        /*await UniTask.WaitUntil(() =>
            _view.StateInfo.IsName("Attack") &&
            _view.StateInfo.normalizedTime >= 0.9f
        , cancellationToken: this.GetCancellationTokenOnDestroy());*/
        rb.velocity = Vector2.zero;
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

        magics[magicNum].CreateMagic(_model, transform.position, magicNum);
        //別のボタンの魔法をほぼ同時に使えないように待機
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        stateCon.DeleteState(WizardState.Magicing);
    }

    private void OnOption(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed
            || context.action.name != "OpenWindow") return;

        UIManager.Instance.ShowOption();
        _playerInput.actions.Disable();
        _playerInput.actions.FindActionMap("UI");
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
        var damage = _model.CalculateDamage(enemyAttack, enemyStrength);
        Debug.Log("damage : " + damage);
        //ダメージ量の記録
        PlayDataRecorder.Instance.AddTotalDamage(damage);
        PlayDataRecorder.Instance.AddDamageCount();
        //体力-ダメージ量が負の値になったら0、そうでないなら体力-ダメージ量をそのまま代入
        _model.HitPoint.Value = _model.DecreaseHitPoint(damage);

        UIManager.Instance.CreateDamageText(transform.position, damage, 2.0f);

        Die();

        //SkillManager.Instance.TriggerOnDamage(conditionManager);

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

        UIManager.Instance.CreateDamageText(transform.position, damage, 2.0f);

        //ダメージ量の記録
        PlayDataRecorder.Instance.AddTotalDamage(damage);
        PlayDataRecorder.Instance.AddDamageCount();
        Die();

        //SkillManager.Instance.TriggerOnDamage(conditionManager);

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
        _model.HitPoint.Value = _model.IncreaseHitPoint(healPoint);

        //SkillManager.Instance.TriggerOnHeal(conditionManager);
    }

    private void Die()
    {
        if (_model.HitPoint.Value > 0) return;
        //PlayerInputへの関数登録の解除
        _playerInput.onActionTriggered -= OnMove;
        _playerInput.onActionTriggered -= OnJump;
        _playerInput.onActionTriggered -= OnMagic;

        AudioManager.Instance.PlaySE(AudioType.gameOver);
        _model.PlayerVelocity = Vector2.zero;
        _view.SetAnimTrigger("die");
        //ゲームオーバー画面の表示
        UIManager.Instance.ShowGameOver();
        _playerInput.actions.Disable();
        _playerInput.actions.FindActionMap("UI");
    }

    //地面と接しているかの判定
    private void IsGround()
    {
        //地面と接していたらStandingを追加
        //if (ground.IsGround(adjustRaycast_y))
        if(raycaster.GetGrounded())
        {
            stateCon.AddState(WizardState.Standing);

            if (stateCon.HasState(WizardState.Jumping) && _model.PlayerVelocityY < 0.0f)
            {
                _view.SetAnimation("isJump", false);
                stateCon.DeleteState(WizardState.Jumping);
            }
        }
        //地面から離れていたらStandingを削除
        else
            stateCon.DeleteState(WizardState.Standing);
    }

    //天井と接しているかの判定
    private void IsCeiling()
    {
        if (raycaster.GetGeilinged())
            stateCon.AddState(WizardState.Ceiling);
        else
            stateCon.DeleteState(WizardState.Ceiling);
    }
}