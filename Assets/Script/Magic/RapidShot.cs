using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using ShotMagicMethod;

public class RapidShot : ShotMagic, IShotMagicEffect
{
    public override void Action()
    {
        SetActionAsync().Forget();
    }

    public void Effect(EnemyModel enemyModel)
    {
        //敵に当たったら無くなる
        Destroy(gameObject, 0.1f);
    }

    //戻り値がvoidの関数内でawaitするための関数
    private async UniTask SetActionAsync()
    {
        //速度の設定
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(_status.Speed * _model.Direction, 0);
        //指定時間が経ったらDestroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
