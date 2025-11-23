using UnityEngine;
using Cysharp.Threading.Tasks;
using Skill;

[CreateAssetMenu(menuName = "Skills/StartBoost")]
public class StartBoostSkill : SkillBase, IOnGameStart
{
    public int strengthUp;
    public int defenseUp;
    public int speedUp;

    public void OnGameStart(MagicCreator[] magicCreators, ConditionManager conditionManager)
    {
        Condition condition = new StartBoost(conditionName, duration, icon, strengthUp, defenseUp, speedUp);
        conditionManager.AddAndRun(condition).Forget();
    }
}
