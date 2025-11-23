using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/PosionThorn")]
public class PosionThornSkill : SkillBase, IOnCollisionEnemy
{
    public void OnCollisionEnemy(EnemyPresenter enemy)
    {
        Condition condition = new Poison(conditionName, duration, icon);
        enemy.ConditionManager.AddAndRun(condition).Forget();
    }
}
