using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/DefenseDown")]

public class DefenseDownSkill : SkillBase, IOnMagicHit
{
    public int down; //ñhå‰óÕÇÃå∏è≠íl

    public void OnMagicHit(MagicHitContext context)
    {
        Condition condition = new DefenseDown(conditionName, duration, icon, down);
        context.enemy.ConditionManager.AddAndRun(condition).Forget();
    }
}
