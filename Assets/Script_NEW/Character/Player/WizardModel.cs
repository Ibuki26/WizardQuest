using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace WizardPlayer
{
    public class WizardModel
    {
        public ReactiveProperty<int> HitPoint; //体力
        private int _strength; //攻撃力
        private int _defense; //防御力
        private float _speed; //x方向の速度
        private float _jump; //y方向の速度
        private int _maxHitPoint; //体力の最大値
        private int _direction; //向いている方向

        public WizardModel(int hp, int strength,int defense, float speed, float jump)
        {
            HitPoint = new ReactiveProperty<int>(hp);
            _strength = strength;
            _defense = defense;
            _speed = speed;
            _jump = jump;
            _maxHitPoint = hp;
            _direction = 1;
        }

        #region getter,setter
        public int Strength
        {
            get { return _strength; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_strengthへの代入が負の値です。");
                    return;
                }

                _strength = value;
            }
        }

        public int Defense
        {
            get { return _defense; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_defenseへの代入が負の値です。");
                    return;
                }

                _defense = value;
            }
        }

        public float Speed
        {
            get { return _speed; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_speedへの代入が負の値です。");
                    return;
                }

                _speed = value;
            }
        }

        public float Jump
        {
            get { return _jump; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_jumpへの代入が負の値です。");
                    return;
                }

                _jump = value;
            }
        }

        public int MaxHitPoint
        {
            get { return _maxHitPoint; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_maxHitPointへの代入が負の値です。");
                    return;
                }

                _maxHitPoint = value;
            }
        }

        public int Direction
        {
            get { return _direction; }
            set
            {
                if(value != 1 && value != -1)
                {
                    Debug.Log("_directionへ正しい値が代入されてません。");
                    return;
                }

                _direction = value;
            }
        }
        #endregion
    }
}
