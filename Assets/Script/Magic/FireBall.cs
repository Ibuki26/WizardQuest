using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class FireBall : ShotMagic
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;

    protected async override void Action(float speed, float destroyTime)
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cts = new CancellationTokenSource();
        //速度の設定
        rb.velocity = new Vector2(speed, 0);
        //指定時間が経ったらDestroy
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(destroyTime), cancellationToken: cts.Token);
            Destroy(gameObject);
        }
        catch
        {
            //Debug.Log("ActionのDestroyがキャンセルされました。");
        }
    }

    public override async void Effect(EnemyPresenter enemy)
    {
        //Actionのawaitをキャンセル
        cts.Cancel();
        anim.SetBool("disappear", true);

        //ノックバックする方向
        int direction = (rb.velocity.x > 0) ? 1 : -1;
        //Enemyをノックバックする
        enemy.KnockBack(direction * 0.8f).Forget();

        rb.velocity = Vector2.zero;

        //アニメーションが終わるまで待機
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("FireBall_disappear")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }
}
