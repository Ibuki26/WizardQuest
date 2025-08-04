using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;
using Aliment;


[CreateAssetMenu(menuName = "Skills/Poison")]
public class PoisonSkill : SkillBase, IOnMagicHit, IPoisonable
{
    private int level= 1;
    private float duration = 5f;

    public void OnMagicHit(MagicHitContext context)
    {
        context.enemy.Aliment.Poison(duration, level).Forget();
    }

    public void LevelUp()
    {
        level++;
    }

    public void LevelDown()
    {
        level--;
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
}
