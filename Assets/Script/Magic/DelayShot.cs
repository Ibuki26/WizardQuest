using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using ShotMagicMethod;

public class DelayShot : ShotMagic, IShotMagicEffect
{
    private float waitTime = 1.0f;

    public override void Action()
    {
        SetActionAsync().Forget();
    }

    public void Effect(EnemyPresenter _)
    {
        //敵に当たったら無くなる
        Destroy(gameObject, 0.1f);
    }

    //戻り値がvoidの関数内でawaitするための関数
    private async UniTask SetActionAsync()
    {
        var rb = GetComponent<Rigidbody2D>();
        //時間差で_status.Directionを使うと待機終了後のプレイヤーの向きが進行方向になってしまうため、ここで保持
        var direction = _model.Direction; 
        //空中で待機
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //移動開始時に音を鳴らす
        AudioManager.Instance.PlaySE(AudioType.delayShot);
        //速度の設定
        rb.velocity = new Vector2(_status.Speed * direction, 0);
        //指定時間が経ったらDestroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
                cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
