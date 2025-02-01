using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class MagicCreatorStatus : MonoBehaviour
    {
        [SerializeField] private float _coolTime; //クールタイムの待機時間
        [SerializeField] private float _destroyTime; //魔法発動から消失までの時間
        [SerializeField] private Vector3 _adjustCreatePoint; //魔法の生成場所の調整
        [SerializeField] private Magic _magic; //生成する魔法
        [SerializeField] private AudioType _shotShoumd; //魔法生成時の再生音
        [SerializeField] private MagicType _type; //魔法の種類
        [SerializeField] private Sprite _image; //魔法のアイコン画像

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

        public MagicType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        //読み取り専用プロパティ
        public Vector3 AdjustCreatePoint => _adjustCreatePoint;
        public Magic Magic => _magic;
        public AudioType ShotSound => _shotShoumd;

        public Sprite Image => _image;
        #endregion
    }
}
