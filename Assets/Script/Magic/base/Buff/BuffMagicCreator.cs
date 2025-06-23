using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BuffMagicCreator : MagicCreator
{
    private BuffMagicCreatorStatus _status; //�������閂�@�̏��

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
        //���@�̐�����SE�Đ�
        AudioManager.Instance.PlaySE(_status.ShotSound);
        //�N�[���^�C����UI�\��
        UIManager.Instance.BuffAndAreaDisplayEffectTime(_status, num);

        //���@�̕\���ƌ���
        _status.Magic.BuffAnimation().Forget();
        _status.Magic.Buff(model, _status.DestroyTime);
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime));

        //���ʂ̏I��
        UIManager.Instance.BuffAndAreaDisplayCoolTime(_status, num);
        _status.Magic.DeactivateAnimation().Forget();
        _status.Magic.Deactivate(model);

        //�N�[���^�C������
        await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
        IsCoolTime = false;
    }

    //BuffEffecter�̎擾�ƃR���g���[���[�̓o�^
    public void Initialize(BuffEffecter buffEffecter)
    {
        _status.Magic.BuffEffecter = buffEffecter;
        _status.Magic.BuffEffecter.SetAnimatiorController(_status.Controller);
    }
}