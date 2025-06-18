using Cysharp.Threading.Tasks;
using System;

public class Slash : ShotMagic
{
    protected override async void Action(float speed, float disappearTime)
    {
        var token = this.GetCancellationTokenOnDestroy();
        //�w�莞�Ԃ��o������Destroy
        await UniTask.Delay(TimeSpan.FromSeconds(disappearTime), cancellationToken: token);
        Destroy(gameObject);
    }

    public override void Effect(EnemyPresenter enemy)
    {
        //�G�ɓ������Ă��A�c�邽�ߏ����Ȃ�
        return;
    }
}
