using UnityEngine;

//�v���C���̖��@�̃X�e�[�^�X
public class MagicCreatorStatus
{
    protected float _coolTime; //�N�[���^�C���̑ҋ@����
    protected float _destroyTime; //���@������������܂ł̎���
    protected AudioType _shotShoumd; //���@�������̍Đ���
    protected Sprite _image; //���@�̃A�C�R���摜

    #region getter,setter
    public float CoolTime
    {
        get { return _coolTime; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_coolTIme�ւ̑�������̒l�ł��B");
                return;
            }
            _coolTime = value;
        }
    }

    public float DestroyTime
    {
        get { return _destroyTime; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_disappearTime�ւ̑�������̒l�ł��B");
                return;
            }
            _destroyTime = value;
        }
    }
    
    public AudioType ShotSound
    {
        get { return _shotShoumd; }
        set { _shotShoumd = value; }
    }

    public Sprite Image
    {
        get { return _image; }
        set { _image = value; }
    }
    #endregion
}
