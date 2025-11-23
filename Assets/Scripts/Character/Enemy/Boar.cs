using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Boar : GroundMovingEnemyPresenter
{
    public float upValue; //スピードアップ時の加算値
    private EnemySightChecker esc;

    public override void ManualStart()
    {
        base.ManualStart();
        esc = GetComponent<EnemySightChecker>();
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
        if (esc.ScanForPlayer(_groundModel.Direction))
        {
            stateCon.AddState(EnemyState.Moving);
            rb2d.velocity = new Vector2((_groundModel.XSpeed + upValue) * _groundModel.Direction, 0);

            await UniTask.WaitUntil(() => !esc.ScanForPlayer(_groundModel.Direction), cancellationToken : cts.Token);
            await WaitAction(2f);

            stateCon.DeleteState(EnemyState.Moving);
        }
        //視界にプレイヤーがいないとき
        else
        {
            rb2d.velocity = new Vector2(_groundModel.XSpeed * _groundModel.Direction, 0);
        }
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        rb2d.velocity = Vector2.zero;
    }

    protected override void CheckGround()
    {
        if(raycaster.GetGrounded())
        {
            stateCon.AddState(EnemyState.Standing);
            return;
        }

        //地面と接していないとき
        stateCon.DeleteState(EnemyState.Standing);
        Fall();

        return;
    }

    protected override void CheckNextGround()
    {
        if (!stateCon.HasState(EnemyState.Standing)) return;

        if(raycaster.GetNextGrounded())
        {
            return;
        }

        //視界にプレイヤーがいるときの挙動のとき、それを解除
        if (stateCon.HasState(EnemyState.Moving))
        {
            StopAsyncTasks();
            stateCon.DeleteState(EnemyState.Moving);
        }
        //地面が無いとき
        _groundModel.Direction *= -1;
        //向きの調整
        _view.FlipXImage(_groundModel.Direction);
 
        return;
    }

    protected override void CheckWallToTurn()
    {
        if (!stateCon.HasState(EnemyState.Standing)) return;

        if(raycaster.GetWalled())
        {
            //視界にプレイヤーがいるときの挙動のとき、それを解除
            if (stateCon.HasState(EnemyState.Moving))
            {
                StopAsyncTasks();
                stateCon.DeleteState(EnemyState.Moving);
            }
            _groundModel.Direction *= -1;
            //向きの調整
            _view.FlipXImage(_groundModel.Direction);
        }

        //壁がないときは何もしない
    }

    protected override void Fall()
    {
        rb2d.velocity = new Vector2(0, _groundModel.Gravity);
    }

    //BoarではMoving状態のときが走っている状態とする
    protected override void SetAttack()
    {
        _groundModel.Attack = stateCon.HasState(EnemyState.Moving) ? 35 : 30;
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
        _groundModel.XSpeed -= 1.5f;
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
        _groundModel.XSpeed += 1.5f;
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
