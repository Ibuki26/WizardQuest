using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using DG.Tweening;
using ShotMagicMethod;

public abstract class EnemyPresenter : MonoBehaviour
{
    public int hp;
    public int strength;
    public int defense;
    public int score;
    public int direction;

    protected EnemyModel _model;
    protected EnemyView _view;
    protected FlagsController<EnemyState> stateCon;
    protected ConditionManager _conditionManager;
    protected CancellationTokenSource cts;

    public EnemyModel Model => _model;


    public ConditionManager ConditionManager => _conditionManager;

    ////InGameのStartで呼ばれる関数 この関数の実質的なStart
    public virtual void ManualStart()
    {
        _model = CreateModel();
        _view = GetComponent<EnemyView>();
        _view.ManualStart();
        stateCon = new FlagsController<EnemyState>();
        _conditionManager = GetComponent<ConditionManager>();
        _conditionManager.ManualStart();
        cts = new CancellationTokenSource();
    }

    //InGameのFixedUpdateで呼ばれる関数 この関数の実質的なFixedUpdate
    public abstract void ManualFixedUpdate();

    private void OnTriggerStay2D(Collider2D collision)
    {
        //プライヤーの魔法に当たったときの処理　ダメージ無視状態なら行わない
        if (collision.TryGetComponent<ShotMagic>(out var shotMagic))
        {
            Damage(shotMagic.Status.Attack, shotMagic.Model.Strength).Forget();
            //shoot型Magicの当たったときの効果
            var method = shotMagic as IShotMagicEffect;
            if (method != null)
                method.Effect(this);
            //スキルの実行
            var damage = _model.CalculateDaage(shotMagic.Status.Attack, shotMagic.Model.Strength);
            var context = new MagicHitContext(shotMagic.Model, shotMagic.Status, this, damage);
            //SkillManager.Instance.TriggerOnMagicHit(context);
        }

        //プレイヤーにダメージを与える処理
        if (collision.transform.parent != null
            && collision.transform.parent.TryGetComponent<WizardPresenter>(out var player))
        {
            //Attackの数値をダメージ前に代入する
            player.DamageFromEnemy(_model.Attack, _model.Strength).Forget();

            //SkillManager.Instance.TriggerOnCollisionEnemy(this);
        }
    }

    protected virtual void OnBecameVisible()
    {
        stateCon.AddState(EnemyState.OnCamera);
    }

    protected virtual void OnBecameInvisible()
    {
        //倒されたときに画面外に出るのはカウントしない
        if (_model.HitPoint == 0) return;
        stateCon.DeleteState(EnemyState.OnCamera);
    }

    private void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    //Enemyがダメージを受ける処理
    private async UniTask Damage(int playerAttack, int playerStrength)
    {
        if (_model.HitPoint < 0
            || stateCon.HasState(EnemyState.IgnoreDamage)) return;

        //魔法が当たった回数に加算
        PlayDataRecorder.Instance.AddMagicHitCount();

        //無敵状態の開始
        stateCon.AddState(EnemyState.IgnoreDamage);
        //ダメージ演出
        AudioManager.Instance.PlaySE(AudioType.damage_moster);
        _view.DamageColor().Forget();
        //ダメージ量の計算
        var resultDamage = _model.CalculateDaage(playerAttack, playerStrength);
        //ダメージ量の記録
        PlayDataRecorder.Instance.AddTotalAttack(resultDamage);
        //ダメージの体力への反映
        _model.HitPoint = _model.DecreaseHitPoint(resultDamage);

        UIManager.Instance.CreateDamageText(transform.position, resultDamage, 0.8f);

        Die();

        Debug.Log("attack : " + playerAttack + ", strength : " + playerStrength + ", result : " + resultDamage);

        //無敵状態の待機
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken : this.GetCancellationTokenOnDestroy());
        //無敵状態の解除
        stateCon.DeleteState(EnemyState.IgnoreDamage);

        return;
    }
    
    //Enemyが定数ダメージを受ける処理
    public async UniTask DamageConstant(int damage)
    {
        if (_model.HitPoint < 0
            || stateCon.HasState(EnemyState.IgnorConstantDamage)) return;

        //無敵状態の開始
        stateCon.AddState(EnemyState.IgnorConstantDamage);
        //ダメージ演出
        AudioManager.Instance.PlaySE(AudioType.damage_moster);
        _view.DamageColor().Forget();
        //ダメージの体力への反映
        _model.HitPoint = _model.DecreaseHitPoint(damage);
        //ダメージ量の記録
        PlayDataRecorder.Instance.AddTotalAttack(damage);

        UIManager.Instance.CreateDamageText(transform.position, damage, 0.8f);

        Die();

        //無敵状態の待機
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken : this.GetCancellationTokenOnDestroy());
        //無敵状態の解除
        stateCon.DeleteState(EnemyState.IgnorConstantDamage);

        return;
    }

    

    //Enemyが倒されたときの処理
    public void Die()
    {
        if (_model.HitPoint > 0) return;

        //敵を倒した回数を記録
        PlayDataRecorder.Instance.AddEnemyDethCount();

        //現在の動作を停止
        StopAsyncTasks();
        //スコアの加算
        UIManager.Instance.AddScore(_model.Score);
        //移動状態の解除
        stateCon.DeleteState(EnemyState.Moving);
        //SEを流す
        AudioManager.Instance.PlaySE(AudioType.die_moster);
        //当たり判定を無くす
        GetComponent<Collider2D>().enabled = false;
        //x方向の動きを止め、下に落とす
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -4.0f);
        //回転のアニメーション
        transform.DORotate(new Vector3(0, 0, -90 * _model.Direction), 1)
            .OnComplete(() => Destroy(gameObject));
    }

    //実行中の動作を停止させる関数
    public void StopAsyncTasks()
    {
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();
        stateCon.DeleteState(EnemyState.Moving);
    }

    //FixedUpdate内でしてい時間待機させる関数
    protected async UniTask WaitAction(float waitTime)
    {
        var timer = 0f;
        while(timer < waitTime)
        {
            timer += Time.fixedDeltaTime;
            await UniTask.WaitForFixedUpdate(cancellationToken : cts.Token);
        }
    }

    //攻撃の威力をセットする関数
    protected abstract void SetAttack();

    //麻痺状態にする関数
    public abstract UniTask Paralysis(float duration);

    //ノックバックする関数
    public abstract UniTask Knockback(Vector2 direction, float force);

    //ステータスのインスタンスの生成
    protected abstract EnemyModel CreateModel();
}
