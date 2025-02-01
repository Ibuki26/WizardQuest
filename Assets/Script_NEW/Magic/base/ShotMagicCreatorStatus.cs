using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class ShotMagicCreatorStatus : MagicCreatorStatus
    {
        [SerializeField] private int _attack; //魔法の威力
        [SerializeField] private float _speed; //魔法の移動速度
        private int _wizardStrength; //プレイヤーのStrengthを記録する変数

        #region getter,setter
        
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

        public int WizardStrength
        {
            get { return _wizardStrength; }
            set { _wizardStrength = value; }
        }
        #endregion //
    }
}
