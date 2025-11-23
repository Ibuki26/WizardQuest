using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class DefenseDown : Condition
{
    public int down;

    public DefenseDown(string name, float time, Sprite icon, int down)
        :base(name, time, icon)
    {
        this.down = down;
    }

    public async override UniTask Effect(WizardPresenter wizard, CancellationToken token)
    {
        //Defenseの数値を減少、時間が経ったら元に戻す
        wizard.Model.Defense -= down;
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: token);
        wizard.Model.Defense += down;
    }

    public async override UniTask Effect(EnemyPresenter enemy, CancellationToken token)
    {
        //Defenseの数値を減少、時間が経ったら元に戻す
        enemy.Model.Defense -= down;
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: token);
        enemy.Model.Defense += down;
    }
}
