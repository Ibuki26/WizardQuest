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
                Debug.Log("coolTIme�ւ̑�������̒l�ł��B");
                return;
            }
            coolTime = value;
        }
    }
    #endregion

    //MagicCreator�N���X�ŊǗ��ł���悤�ɃC���X�^���X�����֐��ɂ���
    public static MagicCreator Initialize(MagicCreatorStatusData data, Func<GameObject, Vector3, Quaternion, GameObject> func)
    {
        //ShotMagic�N���X�p��Creator�̍쐬
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

    //�eMagicType���Ƃœ��e��ύX
    public abstract void CreateMagic(WizardModel playerModel, Vector3 position, int num);
}
