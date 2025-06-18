using Cysharp.Threading.Tasks;
using System;

public class ShotMagicCreator : MagicCreator
{
    private ShotMagicCreatorStatus _status;

    public ShotMagicCreatorStatus Status => _status;

    public ShotMagicCreator(ShotMagicCreatorStatus status)
    {
        _status = status;
    }

    public async override void CreateMagic(WizardPresenter player, int num)
    {
        if (IsCoolTime) return;
        IsCoolTime = true;
        //MagicのStrengthの更新
        _status.WizardStrength = player.Model.Strength;
        //魔法の生成とSE再生
        AudioManager.Instance.PlaySE(_status.ShotSound);
        ShotMagic shotMagic = (ShotMagic)_status.Magic;
        //クールタイムのUI表示
        UIManager.Instance.ShotDisplayCoolTime(_status, num);
        shotMagic.Create(_status, player.transform.position, player.Model.Direction);
        //クールタイム処理
        await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
        IsCoolTime = false;
    }
}
