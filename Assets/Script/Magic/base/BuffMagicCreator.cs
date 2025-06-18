using Cysharp.Threading.Tasks;
using System;

public class BuffMagicCreator : MagicCreator
{
    private MagicCreatorStatus _status;

    public MagicCreatorStatus Status => _status;

    public BuffMagicCreator(MagicCreatorStatus status)
    {
        _status = status;
    }

    public async override void CreateMagic(WizardPresenter player, int num)
    {
        if (IsCoolTime) return;
        IsCoolTime = true;
        //���@�̐�����SE�Đ�
        AudioManager.Instance.PlaySE(_status.ShotSound);
        BuffMagic buffMagic = (BuffMagic)_status.Magic;
        //�N�[���^�C����UI�\��
        UIManager.Instance.BuffAndAreaDisplayEffectTime(_status, num);
        buffMagic.Create(_status, player, _status.DestroyTime);
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime));
        //���ʂ̏I��
        UIManager.Instance.BuffAndAreaDisplayCoolTime(_status, num);
        //�N�[���^�C������
        await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
        IsCoolTime = false;
    }
}
