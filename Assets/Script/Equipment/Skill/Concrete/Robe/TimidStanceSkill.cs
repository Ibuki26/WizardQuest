using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Skill;

[CreateAssetMenu(menuName = "Skills/TimidStance")]
public class TimidStanceSkill : SkillBase, IOnDamage, IOnGameStart
{
    public int up;
    public float duration;
    private bool flag = false;

    public void OnGameStart(MagicCreator[] magicCreators, WizardModel model)
    {
        flag = false;
    }

    public void OnDamage(WizardModel model)
    {
        AsyncEffect(model).Forget();
    }

    private async UniTask AsyncEffect(WizardModel model)
    {
        if (flag) return;

        Debug.Log("timid on");
        flag = true;
        model.Speed += up;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        model.Speed -= up;
        flag = false;
        Debug.Log("timid off");
    }
}
