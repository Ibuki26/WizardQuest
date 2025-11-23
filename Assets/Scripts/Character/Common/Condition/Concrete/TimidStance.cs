using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

public class TimidStance : Condition
{
    public int up;

    public TimidStance(string name, float time, Sprite icon, int up)
        :base(name, time, icon)
    {
        this.up = up;
    }

    public async override UniTask Effect(WizardPresenter wizard, CancellationToken token)
    {
        wizard.Model.Speed += up;
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: token);
        wizard.Model.Speed -= up;
    }

    //仮実装。速さはキャラクターによって無いため、実装するときに修正
    public async override UniTask Effect(EnemyPresenter enemy, CancellationToken token)
    {
        //enemy.Model.Speed += up;
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: token);
        //enemy.Model.Speed -= up;
    }
}
