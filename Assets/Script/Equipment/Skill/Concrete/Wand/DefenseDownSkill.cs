using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/DefenseDown")]

public class DefenseDownSkill : SkillBase, IOnMagicHit
{
    public int down; //–hŒä—Í‚ÌŒ¸­’l
    public float duration; //Œø‰ÊŠÔ

    public void OnMagicHit(MagicHitContext context)
    {
        context.enemy.Aliment.DefenseDown(down, duration).Forget();
    }
}
