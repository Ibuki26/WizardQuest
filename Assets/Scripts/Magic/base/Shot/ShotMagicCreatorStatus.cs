using UnityEngine;

public class ShotMagicCreatorStatus : MagicCreatorStatus
{
    private ShotMagic _magic; //生成する魔法
    private Vector3 _adjustCreatePosition; //魔法の生成場所の調整
    private int _attack; //魔法の威力
    private float _speed; //魔法の移動速度

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
                Debug.Log("_attackへの代入が負の値です。");
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
                //反対方向への移動はdirectionで管理するためspeedは正の値で管理
                Debug.Log("_speedへの代入が負の値です。");
                return;
            }
            _speed = value;
        }
    }
    #endregion
}