using UnityEngine;

public class GroundMovingEnemyModel : EnemyModel
{
    private float _xSpeed; //移動スピード
    private float _moveTime; //移動時間
    private float _gravity; //落ちる速さ

    //インスタンスの生成
    public GroundMovingEnemyModel
        (int hp, int strength, int defense, int score, int direction,
        float speed, float moveTime, float gravity) : base(hp, strength, defense, score, direction)
    {
        _xSpeed = speed;
        _moveTime = moveTime;
        _gravity = gravity;
    }

    public float XSpeed
    {
        get { return _xSpeed; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_xSpeedへの代入が負の値です。");
                return;
            }

            _xSpeed = value;
        }
    }

    public float MoveTime
    {
        get { return _moveTime; }
        set
        {
            if(value < 0)
            {
                Debug.Log("_moveTimeへの代入が負の値です。");
                return;
            }

            _moveTime = value;
        }
    }

    public float Gravity
    {
        get { return _gravity; }
        set
        {
            if(value > 0)
            {
                Debug.Log("gravityへの代入が正の値です。");
                return;
            }

            _gravity = value;
        }
    }
}
