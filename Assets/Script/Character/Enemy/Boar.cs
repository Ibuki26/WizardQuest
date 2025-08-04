using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Boar : GroundMovingEnemyPresenter
{
    [SerializeField] private float upValue; //スピードアップ時の加算値
    private EnemySightChecker esc;

    //Raycastの調整用数値
    private float adjustRaycastX_turn = 0.95f;
    private float adjustRaycastGround = 0.04f;
    private float adjustRaycastWall = 0.53f;

    public override void ManualStart()
    {
        base.ManualStart();
        esc = GetComponent<EnemySightChecker>();
        esc.Initialize();
        _view.FlipXImage(_model.Direction);
    }

    protected override void Move()
    {
        SetMoveAsync().Forget();
    }
    
    //FixedUpdate内でawaitするための関数
    private async UniTask SetMoveAsync()
    {
        if (!stateCon.HasState(EnemyState.Standing)
            || stateCon.HasState(EnemyState.Moving)) return;

        //視界にプレイヤ―がいるとき
        if (esc.IsVisible(GroundModel))
        {
            stateCon.AddState(EnemyState.Moving);
            rb2d.velocity = new Vector2((GroundModel.XSpeed + upValue) * GroundModel.Direction, 0);

            await UniTask.WaitUntil(() => !esc.IsVisible(GroundModel), cancellationToken : cts.Token);
            await WaitAction(2f);

            stateCon.DeleteState(EnemyState.Moving);
        }
        //視界にプレイヤーがいないとき
        else
        {
            rb2d.velocity = new Vector2(GroundModel.XSpeed * GroundModel.Direction, 0);
        }
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
        Fall();

        return;
    }

    protected override void CheckGroundToTurn()
    {
        if (!stateCon.HasState(EnemyState.Standing)) return;

        var adjustPosition_x = transform.position.x + adjustRaycastX_turn * GroundModel.Direction;
        var adjustPosition_y = transform.position.y - adjustRaycastGround;
        //地面とあるとき
        if (RaycastHelper.CheckGroundAndWalls(new Vector2(adjustPosition_x, adjustPosition_y), Vector2.down, 0.1f, Color.red))
        {
            return;
        }

        //地面が無いとき
        GroundModel.Direction *= -1;
        //向きの調整
        _view.FlipXImage(GroundModel.Direction);
        return;
    }

    protected override void CheckWallToTurn()
    {
        if (!stateCon.HasState(EnemyState.Standing)) return;

        //Raycastの発生位置の調整
        var adjustPosition_x = transform.position.x + adjustRaycastX_turn * GroundModel.Direction;
        var adjustPosition_y = transform.position.y + adjustRaycastWall;
        var direction = new Vector2(GroundModel.Direction, 0);
        //壁があるとき
        if (RaycastHelper.CheckGroundAndWalls(new Vector2(adjustPosition_x, adjustPosition_y), direction, 0.1f, Color.red))
        {
            GroundModel.Direction *= -1;
            //向きの調整
            _view.FlipXImage(GroundModel.Direction);
        }

        //壁がないときは何もしない
    }

    protected override void Fall()
    {
        rb2d.velocity = new Vector2(0, GroundModel.Gravity);
    }

    //BoarではMoving状態のときが走っている状態とする
    protected override void SetAttack()
    {
        GroundModel.Attack = stateCon.HasState(EnemyState.Moving) ? 30 : 18;
    }

    public async override UniTask Paralysis(float duration)
    {
        if (stateCon.HasState(EnemyState.Paralysised)) return;

        stateCon.AddState(EnemyState.Paralysised);
        //動きの停止
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        rb2d.velocity = Vector2.zero;
        //速さの低下
        GroundModel.XSpeed -= 1.5f;
        //スタン
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //動きの再開
        stateCon.DeleteState(EnemyState.Stopped);
        //麻痺が切れるまでの待機
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //速さをもとに戻す
        stateCon.DeleteState(EnemyState.Paralysised);
        GroundModel.XSpeed += 1.5f;
    }

    public async override UniTask Knockback(Vector2 direction, float force)
    {
        //動きの停止
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        rb2d.velocity = Vector2.zero;
        //ノックバック
        rb2d.AddForce(direction * force, ForceMode2D.Impulse);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        stateCon.DeleteState(EnemyState.Stopped);
    }
}
