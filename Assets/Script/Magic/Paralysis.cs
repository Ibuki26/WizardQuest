using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class Paralysis : ShotMagic
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;

    protected override async void Action(float speed, float destroyTime)
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

    public async override void Effect(EnemyPresenter enemy)
    {
        //Actionのawaitをキャンセル
        cts.Cancel();

        //音声再生とアニメーションの再生
        AudioManager.Instance.PlaySE(AudioType.paralysis);
        rb.velocity = Vector2.zero;
        anim.SetBool("hit", true);

        //Enemyの動きを止める
        enemy.Stop(3.0f).Forget();

        //アニメーションが終わるまで待機
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("Paralysis_hit")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }
}
