using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using WizardPlayer;

namespace WizardMagic
{
    public abstract class MagicCreator
    {
        private bool isCoolTime;
        private float coolTime;

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

        //MagicCreator�N���X�ŊǗ��ł���悤�ɃC���X�^���X�����֐��ɂ���
        public static MagicCreator Initialize(MagicCreatorStatus status)
        {
            switch (status.Type)
            {
                case MagicType.shot:
                    MagicCreator magicCreator = new ShotMagicCreator((ShotMagicCreatorStatus)status);
                    magicCreator.coolTime = status.CoolTime;
                    return magicCreator;
                case MagicType.buff:
                    MagicCreator magicCreator2 = new BuffMagicCreator(status);
                    magicCreator2.coolTime = status.CoolTime;
                    return magicCreator2;
                case MagicType.area:
                    MagicCreator magicCreator3 = new AreaMagicCreator((AreaMagicCreatorStatus)status);
                    magicCreator3.coolTime = status.CoolTime;
                    return magicCreator3;
                default:
                    return null;
            }
        }

        //�eMagicType���Ƃœ��e��ύX
        public abstract void CreateMagic(WizardPresenter player, int num);
    }
}
