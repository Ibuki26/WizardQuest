using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using ShotMagicMethod;

public class Paralysis : ShotMagic, IShotMagicEffect
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;
    private bool hasDispose = false; //Effect後に二重にDisposeをするのを回避するため

    public override void Action()
    {
        SetActionAsync().Forget();
    }

    public void Effect(EnemyPresenter enemy)
    {
        if (enemy.Model.HitPoint != 0)
            SetEffectAsync(enemy).Forget();
        else
            Destroy(gameObject);
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
    private async UniTask SetEffectAsync(EnemyPresenter enemy)
    {
        //Actionのawaitをキャンセル
        cts?.Cancel();
        cts?.Dispose();
        hasDispose = true;

        GetComponent<Collider2D>().enabled = false;
        //音声再生とアニメーションの再生
        AudioManager.Instance.PlaySE(AudioType.paralysis);
        rb.velocity = Vector2.zero;
        anim.SetBool("hit", true);

        //Enemyの動きを止める
        enemy.Paralysis(3f).Forget();

        //アニメーションが終わるまで待機
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("Paralysis_hit")
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
