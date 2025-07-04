using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using ShotMagicMethod;

public class FireBall : ShotMagic, IShotMagicEffect
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;
    private bool hasDispose = false; //Effect後に二重にDisposeをするのを回避するため

    public override void Action()
    {
        SetActionAsync().Forget();
    }

    public void Effect(EnemyModel enemyModel)
    {
        SetEffectAsync(enemyModel).Forget();
    }

    //戻り値がvoidの関数内でawaitするための関数
    private async UniTask SetActionAsync()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cts = new CancellationTokenSource();
        //速度の設定
        rb.velocity = new Vector2(_status.Speed * _model.Direction, 0);
        //指定時間が経ったらDestroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime), cancellationToken: cts.Token);
        Destroy(gameObject);
    }

    //戻り値がvoidの関数内でawaitするための関数
    private async UniTask SetEffectAsync(EnemyModel enemyModel)
    {
        //Actionのawaitをキャンセル
        cts?.Cancel();
        cts?.Dispose();
        hasDispose = true;
        anim.SetBool("disappear", true);

        //ノックバックする方向
        int direction = (rb.velocity.x > 0) ? 1 : -1;
        //Enemyをノックバックする
        //enemyModel.KnockBack(direction * 0.8f).Forget();

        rb.velocity = Vector2.zero;

        //アニメーションが終わるまで待機
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("FireBall_disappear")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (!hasDispose)
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}
