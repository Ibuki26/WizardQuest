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
        private Vector2 _playerVelocity; //Rigidbody2Dに代入する値

        private float standardSpeed = 4.0f; //x方向の移動速度の基準値

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
                    Debug.Log("_playerVelocity.xへの代入が負の値です。");
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
                    Debug.Log("_playerVelocity.yへの代入が負の値です。");
                    return;
                }

                _playerVelocity.y = value;
            }
        }
        #endregion

        //プレイヤーのx方向の速度の計算
        public float RunSpeed(float speed, int direction)
        {
            return standardSpeed * speed / 4 * direction;
        }

        //プレイヤーが敵キャラからダメージを受けるときの計算
        public int CalculateDamage(int attack, int strenght)
        {
            int result = attack + strenght / 5 - _defense / 10;

            //ダメ―ジ量が正の値ならそのまま返し、負の値なら0を返す
            if (result >= 0)
                return result;
            else
                return 0;
        }

        //体力-ダメージが正の値か負の値か確かめる
        public int DecreaseHitPoint(int damage)
        {
            return (HitPoint.Value - damage < 0) ? 0 : HitPoint.Value - damage;
        }

        //体力＋回復量が上限値を超えるか確認する
        public int IncreaseHitPoint(int heal)
        {
            return (HitPoint.Value + heal > MaxHitPoint) ? MaxHitPoint : HitPoint.Value + heal;
        }
    }
}
