using UnityEngine;
using Cysharp.Threading.Tasks;

public class Boost : BuffMagic
{
    public override void Buff(WizardModel model, float destroyTime)
    {
        //PlayerÇÃStatusã≠âª
        model.Strength += 25;
        model.Defense += 30;
        model.Speed += 30;
    }

    public override void Deactivate(WizardModel model)
    {
        AudioManager.Instance.PlaySE(AudioType.boost_down);
        //PlayerÇÃStatusã≠âªÇÃâèú
        model.Strength -= 25;
        model.Defense -= 30;
        model.Speed -= 30;
    }

    public async override UniTask BuffAnimation()
    {
        buffEffecter.gameObject.SetActive(true);
        buffEffecter.SetAnimation("buff", true);
        await buffEffecter.WaitAnimation("Hit-1 Animation");
        buffEffecter.SetAnimation("buff", false);
        buffEffecter.gameObject.SetActive(false);
    }

    public async override UniTask DeactivateAnimation()
    {
        buffEffecter.gameObject.SetActive(true);
        buffEffecter.SetAnimation("deactivate", true);
        await buffEffecter.WaitAnimation("Hit-3 Animation");
        buffEffecter.SetAnimation("deactivate", false);
        buffEffecter.gameObject.SetActive(false);
    }
}
