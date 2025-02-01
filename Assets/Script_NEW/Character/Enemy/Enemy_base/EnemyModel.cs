using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace WizardEnemy
{
    public class EnemyModel
    {
        private int _hitPoint;
        private int _strength;
        private int _defense;
        private float _xSpeed;
        private float _ySpeed;
        private float _moveSpeed;
        private int _score;
        private int _direction;
        private int _attack;
        private EnemyControlState _currentState;

        public EnemyModel(int hp, int strength, int defense, float xSpeed, float ySpeed, float moveSpeed, int score, int direction)
        {
            HitPoint = hp;
            _strength = strength;
            _defense = defense;
            _xSpeed = xSpeed;
            _ySpeed = ySpeed;
            _moveSpeed = moveSpeed;
            _score = score;
            _direction = direction;
            _currentState = 0000;
        }

        #region getter,setter
        public int HitPoint
        {
            get { return _hitPoint; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_hitPoint�ւ̑�������̒l�ł��B");
                    return;
                }

                _hitPoint = value;
            }
        }

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

        public float XSpeed
        {
            get { return _xSpeed; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_xSpeed�ւ̑�������̒l�ł��B");
                    return;
                }

                _xSpeed = value;
            }
        }

        public float YSpeed
        {
            get { return _ySpeed; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_ySpeed�ւ̑�������̒l�ł��B");
                    return;
                }

                _ySpeed = value;
            }
        }

        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_moveSpeed�ւ̑�������̒l�ł��B");
                    return;
                }

                _moveSpeed = value;
            }
        }

        public int Score => _score;

        public int Direction
        {
            get { return _direction; }
            set
            {
                if (value != 1 && value != -1)
                {
                    Debug.Log("_direction�֐������l���������Ă܂���B");
                    return;
                }

                _direction = value;
            }
        }

        public int Attack
        {
            get { return _attack; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_attack�ւ̑�������̒l�ł��B");
                    return;
                }

                _attack = value;
            }
        }

        public EnemyControlState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }
        #endregion
    }
}
