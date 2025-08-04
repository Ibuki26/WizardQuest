using UnityEngine;

public abstract class GroundMovingEnemyPresenter : EnemyPresenter
{
    [SerializeField] private float speed; //x方向の移動する速さ
    [SerializeField] private float moveTime; //移動する時間
    [SerializeField] private float gravity; //落ちる速さ

    protected GroundMovingEnemyModel GroundModel => _model as GroundMovingEnemyModel;

    protected Rigidbody2D rb2d;

    public override void ManualStart()
    {
        base.ManualStart();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public override void ManualFixedUpdate()
    {
        //画面外にいるときと体力が0のときは実行しない
        if (!stateCon.HasState(EnemyState.OnCamera)
            || _model.HitPoint == 0) return;

        //壁や床の確認
        CheckGround();

        if (stateCon.HasState(EnemyState.Stopped)) return;

        CheckGroundToTurn();
        CheckWallToTurn();
        //攻撃の威力の設定
        SetAttack();
        //移動
        Move();
    }

    //移動する関数
    protected abstract void Move();

    //自身が地面の上にいるか確認する関数
    protected abstract void CheckGround();

    //移動先に地面があるか確認する関数
    protected abstract void CheckGroundToTurn();

    //移動先に壁があるか確認する関数
    protected abstract void CheckWallToTurn();

    //落下する関数
    protected abstract void Fall();

    //Rigidbody2Dの起動の追加 
    protected override void OnBecameVisible()
    {
        base.OnBecameVisible();
        rb2d.WakeUp();
    }

    //Rigidbody2Dのスリープの追加
    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        rb2d.Sleep();
    }

    protected override EnemyModel CreateModel()
    {
       return  _model = new GroundMovingEnemyModel(hp, strength, defense, score, direction, speed, moveTime, gravity);
    }
}
