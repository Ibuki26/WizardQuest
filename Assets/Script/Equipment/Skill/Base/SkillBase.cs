using UnityEngine;

//�X�L���̊�{�N���X�B������p������ScriptableObject���C���X�y�N�^�[�ő����ɓo�^����
public class SkillBase : ScriptableObject
{
    public string Name; //�X�L���̖��O
    protected WizardModel _model; //�v���C���[�̃f�[�^

    //�f�[�^�̓o�^
    public void Initialize(WizardModel model)
    {
        _model = model;
    }
}