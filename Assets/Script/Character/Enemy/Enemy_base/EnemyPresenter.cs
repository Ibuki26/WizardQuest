using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;
using ShotMagicMethod;

public abstract class EnemyPresenter : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int strength;
    [SerializeField] private int defense;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int score;
    [SerializeField] private int direction;

    private EnemyModel _model;
    private EnemyView _view;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Tween tween { get; set; }
    private Vector2 enemyVelocity = Vector2.zero;

    public EnemyModel Model => _model;
    public EnemyView View => _view;
    public Rigidbody2D RB => rb;
    public Tween Tween
    {
        get { return tween; }
        set { tween = value; }
    }

    ////InGameのStartで呼ばれる関数 この関数の実質的なStart
    public virtual void ManualStart()
    {
        _model = new EnemyModel(hp, strength, defense, xSpeed, ySpeed, moveSpeed, score, direction);
        _view = GetComponent<EnemyView>();
        _view.ManualStart();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        //向きの調整
        transform.localScale = new Vector3(transform.localScale.x * _model.Direction, transform.localScale.y, transform.localScale.z);
    }

    //InGameのFixedUpdateで呼ばれる関数 この関数の実質的なFixedUpdate
    public abstract void ManualFixedUpdate();

    private void OnTriggerStay2D(Collider2D collision)
    {
        //プライヤーの魔法に当たったときの処理　ダメージ無視状態なら行わない
        if (collision.TryGetComponent<ShotMagic>(out var shotMagic)
            && !_model.CurrentState.HasFlag(EnemyControlState.IgnoreDamage))
        {
            Damage(shotMagic.Status.Attack, shotMagic.Model.Strength).Forget();
            //shoot型Magicの当たったときの効果
            var method = shotMagic as IShotMagicEffect;
            if (method != null)
                method.Effect(_model);
            //スキルの実行
            var context = new MagicHitContext(shotMagic.Model, shotMagic.Status, _model, 0);
            SkillManager.Instance.TriggerOnMagicHit(context);
        }

        //AreaMagicが未実装のため未完成
        if (collision.TryGetComponent<AreaMagic>(out var areaMagic)
            && !_model.CurrentState.HasFlag(EnemyControlState.IgnoreDamage))
        {

        }

        //プレイヤーにダメージを与える処理
        if (collision.transform.parent != null
            && collision.transform.parent.TryGetComponent<WizardPresenter>(out var player))
        {
            //Attackの数値をダメージ前に代入する
            player.DamageFromEnemy(_model.Attack, _model.Strength).Forget();
        }
    }

    private void OnBecameVisible()
    {
        _model.CurrentState |= EnemyControlState.OnCamera;
    }

    private void OnBecameInvisible()
    {
        _model.CurrentState &= ~EnemyControlState.OnCamera;
    }

    //Enemyがダメージを受ける処理
    private async UniTask Damage(int playerAttack, int playerStrength)
    {
        if (_model.HitPoint < 0) return;

        //無敵状態の開始
        _model.CurrentState |= EnemyControlState.IgnoreDamage;
        //ダメージ演出
        AudioManager.Instance.PlaySE(AudioType.damage_moster);
        _view.DamageColor().Forget();
        //ダメージ量の計算
        var resultDamage = playerAttack + playerStrength / 5 - _model.Defense / 10;
        //体力-ダメージ量が負の値になったら0、そうでないなら体力-ダメージ量をそのまま代入
        _model.HitPoint = (_model.HitPoint - resultDamage < 0)
            ? 0 : _model.HitPoint - resultDamage;

        Die().Forget();

        Debug.Log("attack : " + playerAttack + ", strength : " + playerStrength + ", resulr : " + resultDamage);

        //無敵状態の待機
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
        //無敵状態の解除
        _model.CurrentState &= ~EnemyControlState.IgnoreDamage;
    }

    //Enemyが倒されたときの処理
    public async UniTask Die()
    {
        if (_model.HitPoint > 0) return;

        //スコアの加算
        UIManager.Instance.AddScore(_model.Score);
        //移動状態の解除
        _model.CurrentState &= ~EnemyControlState.Moving;

        //現在実行中のアニメーションを停止
        tween.Kill();
        AudioManager.Instance.PlaySE(AudioType.die_moster);
        coll.enabled = false;
        //x方向の動きを止め、下に落とす
        enemyVelocity = new Vector2(0, -4.0f);
        rb.velocity = enemyVelocity;
        //回転のアニメーション
        tween = transform.DORotate(new Vector3(0, 0, -90 * _model.Direction), 1);
        //スコアの追加
        //カメラに映らなくなったら、アニメーションを終え、破壊
        await UniTask.WaitUntil(() => !_model.CurrentState.HasFlag(EnemyControlState.OnCamera));
        tween.Kill();
        Destroy(gameObject);
    }

    //Enemyを一定時間止める関数
    public async UniTask Stop(float stopTime)
    {
        if (_model.CurrentState.HasFlag(EnemyControlState.Stopped)) return;
        if (_model.HitPoint == 0) return;

        //停止状態の開始
        _model.CurrentState |= EnemyControlState.Stopped;
        _model.CurrentState &= ~EnemyControlState.Moving;
        StopOrder(stopTime);
        //停止状態の待機
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(stopTime), cancellationToken: token);
        //停止状態の解除
        _model.CurrentState &= ~EnemyControlState.Stopped;
    }

    //Enemyをノックバックする関数
    public async UniTask KnockBack(float x)
    {
        if (_model.HitPoint == 0) return;

        //すぐ動き出さないように停止状態にする
        _model.CurrentState |= EnemyControlState.Stopped;
        //直前にやっていたDoTweenアニメーションの破棄
        tween.Kill();
        tween = transform.DOMoveX(transform.position.x + x, 0.2f, false);
        //少し待って行動の再開
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: this.GetCancellationTokenOnDestroy());
        //x方向移動のアニメーションを破棄
        //tween.Kill();
        _model.CurrentState &= ~EnemyControlState.Moving;
        _model.CurrentState &= ~EnemyControlState.Stopped;
    }

    public abstract void StopOrder(float stopTime);
}
