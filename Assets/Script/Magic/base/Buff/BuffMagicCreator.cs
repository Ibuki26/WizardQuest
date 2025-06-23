using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BuffMagicCreator : MagicCreator
{
    private BuffMagicCreatorStatus _status; //生成する魔法の情報

    public BuffMagicCreatorStatus Status => _status;

    public BuffMagicCreator(BuffMagicCreatorStatusData data)
    {
        _status = new BuffMagicCreatorStatus(data);
    }

    public override void CreateMagic(WizardModel playerModel, Vector3 position, int num)
    {
        SetCreateMagicAsync(playerModel, num).Forget();
    }

    private async UniTask SetCreateMagicAsync(WizardModel model, int num)
    {
        if (IsCoolTime) return;
        IsCoolTime = true;
        //魔法の生成とSE再生
        AudioManager.Instance.PlaySE(_status.ShotSound);
        //クールタイムのUI表示
        UIManager.Instance.BuffAndAreaDisplayEffectTime(_status, num);

        //魔法の表示と効果
        _status.Magic.BuffAnimation().Forget();
        _status.Magic.Buff(model, _status.DestroyTime);
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime));

        //効果の終了
        UIManager.Instance.BuffAndAreaDisplayCoolTime(_status, num);
        _status.Magic.DeactivateAnimation().Forget();
        _status.Magic.Deactivate(model);

        //クールタイム処理
        await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
        IsCoolTime = false;
    }

    //BuffEffecterの取得とコントローラーの登録
    public void Initialize(BuffEffecter buffEffecter)
    {
        _status.Magic.BuffEffecter = buffEffecter;
        _status.Magic.BuffEffecter.SetAnimatiorController(_status.Controller);
    }
}