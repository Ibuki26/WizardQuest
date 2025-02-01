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
                    Debug.Log("coolTImeへの代入が負の値です。");
                    return;
                }
                coolTime = value;
            }
        }

        //MagicCreatorクラスで管理できるようにインスタンス化を関数にした
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

        //各MagicTypeごとで内容を変更
        public abstract void CreateMagic(WizardPresenter player, int num);
    }
}
