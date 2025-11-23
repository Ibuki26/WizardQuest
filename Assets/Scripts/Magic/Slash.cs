using Cysharp.Threading.Tasks;
using System;

public class Slash : ShotMagic
{
    public override void Action()
    {
        SetActionAsync().Forget();
    }

    //–ß‚è’l‚ªvoid‚ÌŠÖ”“à‚Åawait‚·‚é‚½‚ß‚ÌŠÖ”
    private async UniTask SetActionAsync()
    {
        //w’èŠÔ‚ªŒo‚Á‚½‚çDestroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
