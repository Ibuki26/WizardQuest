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
        private Vector2 _playerVelocity; //Rigidbody2D�ɑ������l

        private float standardSpeed = 4.0f; //x�����̈ړ����x�̊�l

        public WizardModel(int hp, int strength,int defense, float speed, float jump)
        {
            HitPoint = new ReactiveProperty<int>(hp);
            _strength = strength;
            _defense = defense;
            _speed = speed;
            _jump = jump;
            _maxHitPoint = hp;
            _direction = 1;
            _playerVelocity = Vector2.zero;
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

        public Vector2 PlayerVelocity
        {
            get { return _playerVelocity; }
            set { _playerVelocity = value; }
        }

        public float PlayerVelocityX
        {
            get { return _playerVelocity.x; }
            set
            {
                if(value < 0)
                {
                    Debug.Log("_playerVelocity.x�ւ̑�������̒l�ł��B");
                    return;
                }

                _playerVelocity.x = value;
            }
        }

        public float PlayerVelocityY
        {
            get { return _playerVelocity.y; }
            set
            {
                if (value < 0)
                {
                    Debug.Log("_playerVelocity.y�ւ̑�������̒l�ł��B");
                    return;
                }

                _playerVelocity.y = value;
            }
        }
        #endregion

        //�v���C���[��x�����̑��x�̌v�Z
        public float RunSpeed(float speed, int direction)
        {
            return standardSpeed * speed / 4 * direction;
        }

        //�v���C���[���G�L��������_���[�W���󂯂�Ƃ��̌v�Z
        public int CalculateDamage(int attack, int strenght)
        {
            int result = attack + strenght / 5 - _defense / 10;

            //�_���\�W�ʂ����̒l�Ȃ炻�̂܂ܕԂ��A���̒l�Ȃ�0��Ԃ�
            if (result >= 0)
                return result;
            else
                return 0;
        }

        //�̗�-�_���[�W�����̒l�����̒l���m���߂�
        public int DecreaseHitPoint(int damage)
        {
            return (HitPoint.Value - damage < 0) ? 0 : HitPoint.Value - damage;
        }

        //�̗́{�񕜗ʂ�����l�𒴂��邩�m�F����
        public int IncreaseHitPoint(int heal)
        {
            return (HitPoint.Value + heal > MaxHitPoint) ? MaxHitPoint : HitPoint.Value + heal;
        }
    }
}
