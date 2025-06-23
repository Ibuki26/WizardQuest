using UnityEngine;

//プレイ中の魔法のステータス
public class MagicCreatorStatus
{
    protected float _coolTime; //クールタイムの待機時間
    protected float _destroyTime; //魔法発動から消失までの時間
    protected AudioType _shotShoumd; //魔法生成時の再生音
    protected Sprite _image; //魔法のアイコン画像

    #region getter,setter
    public float CoolTime
    {
        get { return _coolTime; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_coolTImeへの代入が負の値です。");
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
                Debug.Log("_disappearTimeへの代入が負の値です。");
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
