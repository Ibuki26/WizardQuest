using UnityEngine;

public class BuffMagicCreatorStatus : MagicCreatorStatus
{
    private BuffMagic _magic; //生成する魔法
    private RuntimeAnimatorController _controller; //演出のアニメーションコントローラー

    public BuffMagicCreatorStatus(BuffMagicCreatorStatusData data)
    {
        _coolTime = data.coolTime;
        _destroyTime = data.destroyTime;
        _shotShoumd = data.shotShoumd;
        _image = data.image;
        _magic = data.magic;
        _controller = data.controller;
    }

    public BuffMagic Magic
    {
        get { return _magic; }
        set { _magic = value; }
    }

    public RuntimeAnimatorController Controller
    {
        get { return _controller; }
        set { _controller = value; }
    }
}
