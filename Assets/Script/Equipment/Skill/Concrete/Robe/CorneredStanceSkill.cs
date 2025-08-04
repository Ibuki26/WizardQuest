using UnityEngine;
using Skill;

[CreateAssetMenu(menuName = "Skills/CorneredStance")]
public class CorneredStanceSkill : SkillBase, IOnGameStart, IOnDamage, IOnHeal
{
    public int strengthUp;
    public int speedUp;
    private bool flag = false;

    public void OnGameStart(MagicCreator[] magicCreators, WizardModel model)
    {
        flag = false;
    }

    public void OnDamage(WizardModel model)
    {
        if (model.HitPoint.Value > model.MaxHitPoint * 0.2f && !flag) return;
        Debug.Log("cornerd");
        flag = true;
        model.Strength += strengthUp;
        model.Speed += speedUp;
    }

    public void OnHeal(WizardModel model)
    {
        if (model.HitPoint.Value <= model.MaxHitPoint * 0.2f && flag) return;
        Debug.Log("cornerd off");
        flag = false;
        model.Strength -= strengthUp;
        model.Speed -= speedUp;
    }
}
