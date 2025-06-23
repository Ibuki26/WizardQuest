using UnityEngine;
using System.Linq;
using Skill;

[CreateAssetMenu(menuName = "Skills/AttackUp_Start")]
public class AttackUpSkill : SkillBase, IOnGameStart
{
    public void OnGameStart(MagicCreator[] magics)
    {
        foreach (var magic in magics.OfType<ShotMagicCreator>()) {
            magic.Status.Attack += 10;
        }
    }
}
