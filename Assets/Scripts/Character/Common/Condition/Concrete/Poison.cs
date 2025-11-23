using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using System.Threading;

public class Poison : Condition
{
    public Poison(string name, float time, Sprite sprite)
        :base(name, time, sprite)
    {
    }

    public async override UniTask Effect(WizardPresenter wizard, CancellationToken token)
    {
        float timer = 0f;
        float waitTime = 2.0f;

        //効果時間の間、一定間隔でダメージを与える
        while (timer <= duration)
        {
            wizard.DamageFromGimmick(2).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime),
                cancellationToken: token);
            timer += waitTime;
        }
    }

    public async override UniTask Effect(EnemyPresenter enemy, CancellationToken token)
    {
        float timer = 0f;
        float waitTime = 2.0f;

        //効果時間の間、一定間隔でダメージを与える
        while(timer <= duration)
        {
            enemy.DamageConstant(2).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime),
                cancellationToken: token);
            timer += waitTime;
        }
    }
}
