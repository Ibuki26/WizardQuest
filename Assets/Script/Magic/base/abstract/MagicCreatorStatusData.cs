using UnityEngine;

//���@�̃X�e�[�^�X�̌��{�f�[�^
//ScriptableObject�ŃC���X�y�N�^�[���炱���o�^���Ďg��
public class MagicCreatorStatusData : ScriptableObject
{
    public float coolTime; //�N�[���^�C���̑ҋ@����
    public float destroyTime; //���@������������܂ł̎���
    public AudioType shotShoumd; //���@�������̍Đ���
    public Sprite image; //���@�̃A�C�R���摜
}