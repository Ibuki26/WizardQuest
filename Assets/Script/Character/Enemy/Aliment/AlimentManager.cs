using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class AlimentManager : MonoBehaviour
{
    private EnemyPresenter enemy;
    private AlimentStateController stateCon;

    public void Initialize(EnemyPresenter enemy)
    {
        this.enemy = enemy;
        stateCon = new AlimentStateController();
    }

    public async UniTask Poison(float duration, int level)
    {
        if (stateCon.HasState(AlimentState.Posion)) return;

        float timer = 0;
        float waitTime = 2f;
        stateCon.AddState(AlimentState.Posion);
        while(timer <= duration){
            enemy.DamageConstant(2 * level).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime),
                cancellationToken: this.GetCancellationTokenOnDestroy());
            timer += waitTime;
        }
        stateCon.DeleteState(AlimentState.Posion);
    }

    public async UniTask DefenseDown(int down, float duration)
    {
        if (stateCon.HasState(AlimentState.DefenseDowm)) return;

        stateCon.AddState(AlimentState.DefenseDowm);
        enemy.Model.Defense -= down;
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        enemy.Model.Defense += down;
        stateCon.DeleteState(AlimentState.DefenseDowm);
    }
}
