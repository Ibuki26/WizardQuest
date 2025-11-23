using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class StartBoost : Condition
{
    public int strengthUp;
    public int defenseUp;
    public int speedUp;

    public StartBoost(string name, float time, Sprite icon, int strength, int defense, int speed)
        :base(name, time, icon)
    {
        strengthUp = strength;
        defenseUp = defense;
        speedUp = speed;
    }

    public async override UniTask Effect(WizardPresenter wizard, CancellationToken token)
    {
        //一定時間ステータスを上昇、時間が過ぎたら元に戻る
        wizard.Model.Strength += strengthUp;
        wizard.Model.Defense += defenseUp;
        wizard.Model.Speed += speedUp;

        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: token);

        wizard.Model.Strength -= strengthUp;
        wizard.Model.Defense -= defenseUp;
        wizard.Model.Speed -= speedUp;
    }

    //仮実装。速さはキャラクターによって無いため、実装するときに修正
    public async override UniTask Effect(EnemyPresenter enemy, CancellationToken token)
    {
        //一定時間ステータスを上昇、時間が過ぎたら元に戻る
        enemy.Model.Strength += strengthUp;
        enemy.Model.Defense += defenseUp;
        //enemy.Model.Speed += speedUp;

        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: token);

        enemy.Model.Strength -= strengthUp;
        enemy.Model.Defense -= defenseUp;
        //enemy.Model.Speed -= speedUp;
    }
}
