using Cysharp.Threading.Tasks;
using System;

public class Slash : ShotMagic
{
    public override void Action()
    {
        SetActionAsync().Forget();
    }

    //戻り値がvoidの関数内でawaitするための関数
    private async UniTask SetActionAsync()
    {
        //指定時間が経ったらDestroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
