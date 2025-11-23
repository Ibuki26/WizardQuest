using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/TimidStance")]
public class TimidStanceSkill : SkillBase, IOnDamage
{
    public int up;

    public void OnDamage(ConditionManager conditionManager)
    {
        Condition condition = new TimidStance(conditionName, duration, icon, up);
        conditionManager.AddAndRun(condition).Forget();
    }
}
