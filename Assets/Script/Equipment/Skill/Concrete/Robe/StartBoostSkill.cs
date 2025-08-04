using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Skill;

[CreateAssetMenu(menuName = "Skills/StartBoost")]
public class StartBoostSkill : SkillBase, IOnGameStart
{
    public int strengthUp;
    public int defenseUp;
    public int speedUp;
    public float duration;

    public void OnGameStart(MagicCreator[] magicCreators, WizardModel model)
    {
        AsyncEffect(model).Forget();
    }

    private async UniTask AsyncEffect(WizardModel model)
    {
        model.Strength += strengthUp;
        model.Defense += defenseUp;
        model.Speed += speedUp;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        model.Strength -= strengthUp;
        model.Defense -= defenseUp;
        model.Speed -= speedUp;
    }
}
