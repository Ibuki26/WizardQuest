using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace WizardPlayer
{
    public class WizardModel
    {
        public ReactiveProperty<int> HitPoint; //�̗�
        private int _strength; //�U����
        private int _defense; //�h���
        private float _speed; //x�����̑��x
        private float _jump; //y�����̑��x
        private int _maxHitPoint; //�̗͂̍ő�l
        private int _direction; //�����Ă������

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
                    Debug.Log("_strength�ւ̑�������̒l�ł��B");
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
                    Debug.Log("_defense�ւ̑�������̒l�ł��B");
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
                    Debug.Log("_speed�ւ̑�������̒l�ł��B");
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
                    Debug.Log("_jump�ւ̑�������̒l�ł��B");
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
                    Debug.Log("_maxHitPoint�ւ̑�������̒l�ł��B");
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
                    Debug.Log("_direction�֐������l���������Ă܂���B");
                    return;
                }

                _direction = value;
            }
        }
        #endregion
    }
}
