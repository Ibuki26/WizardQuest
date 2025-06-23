using UnityEngine;

public abstract class ShotMagic : MonoBehaviour
{
    protected ShotMagicCreatorStatus _status; //���@�̏��
    protected WizardModel _model; //�v���C���[�̏��

    public ShotMagicCreatorStatus Status => _status;
    public WizardModel Model => _model;

    //�ϐ��ւ̏��̓o�^
    public void Initialize(ShotMagicCreatorStatus status, WizardModel model)
    {
        _status = status;
        _model = model;
    }

    //������̖��@�̋���
    public abstract void Action();
}
