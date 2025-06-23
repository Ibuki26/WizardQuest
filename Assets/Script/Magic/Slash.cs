using Cysharp.Threading.Tasks;
using System;

public class Slash : ShotMagic
{
    public override void Action()
    {
        SetActionAsync().Forget();
    }

    //�߂�l��void�̊֐�����await���邽�߂̊֐�
    private async UniTask SetActionAsync()
    {
        //�w�莞�Ԃ��o������Destroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
