using UnityEngine;

public class ShotMagicCreatorStatus : MagicCreatorStatus
{
    private ShotMagic _magic; //�������閂�@
    private Vector3 _adjustCreatePosition; //���@�̐����ꏊ�̒���
    private int _attack; //���@�̈З�
    private float _speed; //���@�̈ړ����x

    public ShotMagicCreatorStatus(ShotMagicCreatorStatusData data)
    {
        _coolTime = data.coolTime;
        _destroyTime = data.destroyTime;
        _adjustCreatePosition = data.adjustCreatePosition;
        _shotShoumd = data.shotShoumd;
        _image = data.image;
        _magic = data.magic;
        _attack = data.attack;
        _speed = data.speed;
    }

    #region getter,setter

    public ShotMagic Magic
    {
        get { return _magic; }
        set { _magic = value; }
    }

    public Vector3 AdjustCreatePoint
    {
        get { return _adjustCreatePosition; }
        set { _adjustCreatePosition = value; }
    }

    public float AdjustCreatePointX
    {
        get { return _adjustCreatePosition.x; }
        set { _adjustCreatePosition.x = value; }
    }

    public int Attack
    {
        get { return _attack; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_attack�ւ̑�������̒l�ł��B");
                return;
            }
            _attack = value;
        }
    }

    public float Speed
    {
        get { return _speed; }
        set
        {
            if (value < 0)
            {
                //���Ε����ւ̈ړ���direction�ŊǗ����邽��speed�͐��̒l�ŊǗ�
                Debug.Log("_speed�ւ̑�������̒l�ł��B");
                return;
            }
            _speed = value;
        }
    }
    #endregion
}