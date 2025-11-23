using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/FollowUpAttack")]
public class FollowUpAttackSkill : SkillBase, IOnMagicHit
{
    public int divider;

    public void OnMagicHit(MagicHitContext context)
    {
        int damage = context.damage / divider;
        context.enemy.DamageConstant(damage).Forget();
    }
}
