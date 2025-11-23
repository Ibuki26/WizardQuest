using UnityEngine;
using Skill;

//ダメージの10分の1回復するスキル
[CreateAssetMenu(menuName = "Skills/Drain")]
public class DrainSkill : SkillBase, IOnMagicHit
{
    public int divier;

    public void OnMagicHit(MagicHitContext context)
    {
        var drainValue = context.damage / divier;
        context.model.HitPoint.Value = context.model.IncreaseHitPoint(drainValue);
    }
}
