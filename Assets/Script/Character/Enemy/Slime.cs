using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Slime : GroundMovingEnemyPresenter
{
    [SerializeField] private float ySpeed;
    //Raycastの調整用数値
    private float adjustRaycastGround = 0.05f;
    private float adjustRaycastWall_x = 0.5f;
    private float adjustRaycastWall_y = 0.35f;

    protected override void Move()
    {
        SetMoveAsync().Forget();
    }

    private async UniTask SetMoveAsync()
    {
        if (stateCon.HasState(EnemyState.Moving)
            || !stateCon.HasState(EnemyState.Standing)) return;

        stateCon.AddState(EnemyState.Moving);
        //山なりの移動の上昇部分
        rb2d.velocity = new Vector2(GroundModel.XSpeed * GroundModel.Direction, ySpeed);
        await WaitAction(GroundModel.MoveTime / 2);
        //山なりの移動の下降部分
        rb2d.velocity = new Vector2(GroundModel.XSpeed * GroundModel.Direction, -ySpeed);
        await WaitAction(GroundModel.MoveTime / 2);
        //一定時間停止
        rb2d.velocity = Vector2.zero;
        await WaitAction(0.5f);
        stateCon.DeleteState(EnemyState.Moving);
    }

    protected override void CheckGround()
    {
        var adjustPosition = new Vector2(transform.position.x, transform.position.y - adjustRaycastGround);
        //地面と接しているとき
        if (RaycastHelper.CheckGroundAndWalls(adjustPosition, Vector2.down, 0.05f, Color.red))
        {
            stateCon.AddState(EnemyState.Standing);
            return;
        }

        //地面と接していないとき
        stateCon.DeleteState(EnemyState.Standing);

        //地面と接触していなくて移動中でもないとき落下
        if (!stateCon.HasState(EnemyState.Moving))
            Fall();

        return;
    }

    protected override void CheckGroundToTurn()
    {
        if (stateCon.HasState(EnemyState.Moving)
            || !stateCon.HasState(EnemyState.Standing)) return;

        //Raycastの発生位置の調整
        var adjustPosition_x = transform.position.x + GroundModel.XSpeed * GroundModel.MoveTime * GroundModel.Direction;
        var adjustPosition_y = transform.position.y - adjustRaycastGround;
        //地面があったとき
        if (RaycastHelper.CheckGroundAndWalls(new Vector2(adjustPosition_x, adjustPosition_y), Vector2.down, 0.1f, Color.red))
        {
            return;
        }

        //何もなかったとき
        GroundModel.Direction *= -1;
    }

    protected override void CheckWallToTurn()
    {
        if (stateCon.HasState(EnemyState.Moving)
            || !stateCon.HasState(EnemyState.Standing)) return;

        //Raycastの発生位置の調整
        var adjustPosition_x = transform.position.x + adjustRaycastWall_x * GroundModel.Direction;
        var adjustPosition_y = transform.position.y + adjustRaycastWall_y;
        var direction = new Vector2(GroundModel.Direction, 0);
        var distance = GroundModel.XSpeed * GroundModel.MoveTime + 0.4f;
        //壁があるとき
        if (RaycastHelper.CheckGroundAndWalls(new Vector2(adjustPosition_x, adjustPosition_y), direction, distance, Color.red))
        {
            GroundModel.Direction *= -1;
        }

        //壁がないときは何もしない
    }

    protected override void Fall()
    {
        rb2d.velocity = new Vector2(0, GroundModel.Gravity);
    }

    protected override void SetAttack()
    {
        GroundModel.Attack = stateCon.HasState(EnemyState.Moving) ? 30 : 20;
    }

    //スライムのParalysisは時間を自由にすると移動中に速度が変わり変な挙動になるため固定
    public async override UniTask Paralysis(float _)
    {
        if (stateCon.HasState(EnemyState.Paralysised)) return;

        //計算と元に戻す用の数値
        var beforeXSpeed = GroundModel.XSpeed;
        var beforeMoveTime = GroundModel.MoveTime;
        var beforeYSpeed = ySpeed;

        stateCon.AddState(EnemyState.Paralysised);
        //動きの停止
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        Fall();
        //速さの低下
        GroundModel.XSpeed -= 1f;
        GroundModel.MoveTime = beforeXSpeed * beforeMoveTime / GroundModel.XSpeed;
        ySpeed = beforeYSpeed * beforeMoveTime / GroundModel.MoveTime;
        //スタン
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //動きの再開
        stateCon.DeleteState(EnemyState.Stopped);
        //麻痺が切れるまでの待機
        await UniTask.Delay(TimeSpan.FromSeconds(beforeMoveTime + 0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //速さをもとに戻す
        stateCon.DeleteState(EnemyState.Paralysised);
        GroundModel.XSpeed = beforeXSpeed;
        GroundModel.MoveTime = beforeMoveTime;
        ySpeed = beforeYSpeed;
    }

    public async override UniTask Knockback(Vector2 direction, float force)
    {
        //動きの停止
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        Fall();
        await UniTask.WaitUntil(() => stateCon.HasState(EnemyState.Standing),
            cancellationToken : this.GetCancellationTokenOnDestroy());
        rb2d.velocity = Vector2.zero;
        //ノックバック
        rb2d.AddForce(direction * force, ForceMode2D.Impulse);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        stateCon.DeleteState(EnemyState.Stopped);
    }
}
