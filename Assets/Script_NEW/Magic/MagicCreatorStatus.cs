using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class MagicCreatorStatus : MonoBehaviour
    {
        [SerializeField] private float _coolTime; //クールタイムの待機時間
        [SerializeField] private int _attack; //魔法の威力
        [SerializeField] private float _speed; //魔法の移動速度
        [SerializeField] private float _disappearTime; //魔法発動から消失までの時間
        [SerializeField] private Vector3 _adjustPos; //魔法の生成場所の調整
        [SerializeField] private int _direction; //魔法の進行方向、向き
        [SerializeField] private Magic _magic; //生成する魔法
        [SerializeField] private AudioType _shotShoumd; //魔法生成時の再生音

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

        public float DisappearTime
        {
            get { return _disappearTime; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_disappearTimeへの代入が負の値です。");
                    return;
                }
                _disappearTime = value;
            }
        }

        public Vector3 AdjustPos
        {
            get { return _adjustPos; }
            set { _adjustPos = value; }
        }

        public int Direction
        {
            get { return _direction; }
            set
            {
                if(value != 1 && value != -1)
                {
                    //方向は向きが正か負かで管理するため数値は1か-1で管理
                    Debug.Log("_directionへの代入が1か-1ではありません");
                    return;
                }
                _direction = value;
            }
        }

        //読み取り専用プロパティ
        public Magic Magic => _magic;
        public AudioType ShotSound => _shotShoumd;
        #endregion //
    }
}
