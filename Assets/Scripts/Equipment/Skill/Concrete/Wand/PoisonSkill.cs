using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;


[CreateAssetMenu(menuName = "Skills/Poison")]
public class PoisonSkill : SkillBase, IOnMagicHit
{
    public void OnMagicHit(MagicHitContext context)
    {
        Condition condition = new Poison(conditionName, duration, icon);
        context.enemy.ConditionManager.AddAndRun(condition).Forget();
    }
}
