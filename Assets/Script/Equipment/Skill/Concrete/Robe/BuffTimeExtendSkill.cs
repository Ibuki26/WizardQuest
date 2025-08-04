using UnityEngine;
using System.Linq;
using Skill;

[CreateAssetMenu(menuName = "Skills/BuffTimeExtend")]
public class BuffTimeExtendSkill : SkillBase, IOnGameStart
{
    public float addDuration;

    public void OnGameStart(MagicCreator[] magicCreators, WizardModel model)
    {
        foreach (var magicCreator in magicCreators.OfType<BuffMagicCreator>())
        {
            magicCreator.Status.DestroyTime += addDuration;
        }
    }
}
