using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class ShotMagicCreator : MagicCreator
{
    private ShotMagicCreatorStatus _status; //�������閂�@�̏��
    private Func<GameObject, Vector3, Quaternion, GameObject> _func;

    #region getter,setter
    public ShotMagicCreatorStatus Status => _status;

    public Func<GameObject, Vector3, Quaternion, GameObject> Func
    {
        get { return _func; }
        set { _func = value; }
    }
    #endregion

    //�R���X�g���N�^
    public ShotMagicCreator(ShotMagicCreatorStatusData data)
    {
        _status = new ShotMagicCreatorStatus(data);
    }

    public async override void CreateMagic(WizardModel model, Vector3 position, int num)
    {
        if (IsCoolTime) return;
        IsCoolTime = true;
        //���@�̐�����SE�Đ�
        AudioManager.Instance.PlaySE(_status.ShotSound);
        //�N�[���^�C����UI�\��
        UIManager.Instance.ShotDisplayCoolTime(_status, num);
        //�������W�̒���
        _status.AdjustCreatePointX = Mathf.Abs(_status.AdjustCreatePointX) * model.Direction;
        var adjustPosition = position + _status.AdjustCreatePoint;
        //Instantiate�֐��ɂ�閂�@�̐���
        var generatedObject = Func?.Invoke(_status.Magic.gameObject, adjustPosition, Quaternion.identity);
        var generatedMagic = generatedObject.GetComponent<ShotMagic>();
        //�����ݒ�Ɛ����������̎��s
        generatedMagic.Initialize(_status, model);
        generatedObject.GetComponent<SpriteRenderer>().flipX = (model.Direction > 0) ? false : true;
        generatedMagic.Action();
        //�N�[���^�C������
        await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
        IsCoolTime = false;
    }
}
