using UnityEngine;

public abstract class GroundMovingEnemyPresenter : EnemyPresenter
{
    public float speed; //x方向の移動する速さ
    public float moveTime; //移動する時間
    public float gravity; //落ちる速さ

    protected GroundMovingEnemyModel _groundModel;
    protected Rigidbody2D rb2d;
    protected CharacterRaycaster raycaster;

    public GroundMovingEnemyModel GroundModel => _groundModel;

    public override void ManualStart()
    {
        base.ManualStart();
        rb2d = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<CharacterRaycaster>();
        raycaster.ManualStart();
        _groundModel = _model as GroundMovingEnemyModel;
    }

    public override void ManualFixedUpdate()
    {
        //画面外にいるときと体力が0のときは実行しない
        if (!stateCon.HasState(EnemyState.OnCamera)
            || _model.HitPoint == 0) return;

        //壁や床の確認
        raycaster.ManualFixedUpdate();
        CheckGround();
        if (stateCon.HasState(EnemyState.Stopped)) return;
        raycaster.CheckNextGroundCollision(_groundModel.Direction);
        CheckNextGround();
        raycaster.CheckWallCollision(_groundModel.Direction);
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
    protected abstract void CheckNextGround();

    //移動先に壁があるか確認する関数
    protected abstract void CheckWallToTurn();

    //落下する関数
    protected abstract void Fall();

    protected override EnemyModel CreateModel()
    {
       return  _model = new GroundMovingEnemyModel(hp, strength, defense, score,
           direction, speed, moveTime, gravity);
    }
}
