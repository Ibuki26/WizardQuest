using UnityEngine;
using System;

public abstract class MagicCreator
{
    private bool isCoolTime;
    private float coolTime;

    #region getter,setter
    public bool IsCoolTime
    {
        get { return isCoolTime; }
        set { isCoolTime = value; }
    }

    public float CoolTime
    {
        get { return coolTime; }
        set
        {
            if (value < 0)
            {
                Debug.Log("coolTImeへの代入が負の値です。");
                return;
            }
            coolTime = value;
        }
    }
    #endregion

    //MagicCreatorクラスで管理できるようにインスタンス化を関数にした
    public static MagicCreator Initialize(MagicCreatorStatusData data, Func<GameObject, Vector3, Quaternion, GameObject> func)
    {
        //ShotMagicクラス用のCreatorの作成
        if(data is ShotMagicCreatorStatusData)
        {
            var magicCreator = new ShotMagicCreator((ShotMagicCreatorStatusData)data);
            magicCreator.CoolTime = data.coolTime;
            magicCreator.Func = func;

            return magicCreator;
        }
        else if(data is BuffMagicCreatorStatusData)
        {
            var magicCreator = new BuffMagicCreator((BuffMagicCreatorStatusData)data);
            magicCreator.CoolTime = data.coolTime;

            return magicCreator;
        }

        return null;
    }

    //各MagicTypeごとで内容を変更
    public abstract void CreateMagic(WizardModel playerModel, Vector3 position, int num);
}
