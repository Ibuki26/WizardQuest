using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/PosionThorn")]
public class PosionThornSkill : SkillBase, IOnCollisionEnemy
{
    public int level;
    public float duration;

    public void OnCollisionEnemy(EnemyPresenter enemy)
    {
        enemy.Aliment.Poison(duration, level).Forget();
    }
}
