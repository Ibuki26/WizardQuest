using UnityEngine;
using Skill;

[CreateAssetMenu(menuName = "Skills/CorneredStance")]
public class CorneredStanceSkill : SkillBase, IOnDamage, IOnHeal
{
    public int strengthUp;
    public int speedUp;

    public void OnDamage(ConditionManager conditionManager)
    {
        CorneredStance condition = new CorneredStance(name, icon, strengthUp, speedUp);
        conditionManager.AddAndRun(condition);
    }

    public void OnHeal(ConditionManager conditionManager)
    {
        CorneredStance condition = new CorneredStance(name, icon, strengthUp, speedUp);
        conditionManager.Remove(condition);
    }
}
