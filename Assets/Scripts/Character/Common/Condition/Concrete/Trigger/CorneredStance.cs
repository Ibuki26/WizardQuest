using UnityEngine;

public class CorneredStance : TriggerCondition
{
    private int strengthUp;
    private int speedUp;

    public CorneredStance(string name, Sprite icon, int strength, int speed)
        :base(name, icon)
    {
        strengthUp = strength;
        speedUp = speed;
    }

    public override bool Effect(WizardPresenter wizard)
    {
        int hp = wizard.Model.HitPoint.Value;
        int maxHp = wizard.Model.MaxHitPoint;
        //HP‚ªÅ‘å‘Ì—Í‚Ì20%‚æ‚è‘å‚«‚¯‚ê‚ÎÀs‚µ‚È‚¢
        if (hp > maxHp * 0.2f) return false;

        wizard.Model.Strength += strengthUp;
        wizard.Model.Speed += speedUp;
        return true;
    }

    public override bool Effect(EnemyPresenter enemy)
    {
        throw new System.NotImplementedException();
    }

    public override bool RemoveEffect(WizardPresenter wizard)
    {
        int hp = wizard.Model.HitPoint.Value;
        int maxHp = wizard.Model.MaxHitPoint;
        //HP‚ªÅ‘å‘Ì—Í‚Ì20%ˆÈ‰º‚¾‚Á‚½‚çÀs‚µ‚È‚¢
        if (hp <= maxHp * 0.2f) return false; ;

        wizard.Model.Strength -= strengthUp;
        wizard.Model.Speed -= speedUp;
        return true;
    }

    public override bool RemoveEffect(EnemyPresenter enemy)
    {
        throw new System.NotImplementedException();
    }
}
