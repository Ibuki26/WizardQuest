using UnityEngine;
using System;

[Serializable]
public class EnemyModel
{
    protected int _hitPoint; //体力
    protected int _strength; //攻撃力
    protected int _defense; //防御力
    protected int _score; //倒されたときに加算さえるスコア
    protected int _direction; //向いている方向
    protected int _attack; //攻撃の威力

    public EnemyModel(int hp, int strength, int defense, int score, int direction)
    {
        _hitPoint = hp;
        _strength = strength;
        _defense = defense;
        _score = score;
        _direction = direction;
    }

    #region getter,setter
    public int HitPoint
    {
        get { return _hitPoint; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_hitPointへの代入が負の値です。");
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
            if (value < 0)
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
            if (value < 0)
            {
                Debug.Log("_defenseへの代入が負の値です。");
                return;
            }

            _defense = value;
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
                Debug.Log("_directionへ正しい値が代入されてません。");
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
            if (value < 0)
            {
                Debug.Log("_attackへの代入が負の値です。");
                return;
            }

            _attack = value;
        }
    }
    #endregion
    //ダメージの計算
    public int CalculateDaage(int attack, int strenght)
    {
        int result = attack + (strenght - _defense) / 4;

        //計算結果が正の値だったら、そのまま返し、負の値なら0を返す
        return (result > 0) ? result : 1;
    }

    //体力とダメージの減算
    public int DecreaseHitPoint(int damage)
    {
        //体力とダメージを計算して、正の値ならそのまま返し、負の値なら0を返す
        return (_hitPoint - damage) < 0 ? 0 : _hitPoint - damage;
    }
}