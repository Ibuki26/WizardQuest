using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Slime : GroundMovingEnemyPresenter
{
    [SerializeField] public float ySpeed;

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
        rb2d.velocity = new Vector2(_groundModel.XSpeed * _groundModel.Direction, ySpeed);
        await WaitAction(_groundModel.MoveTime / 2);
        //山なりの移動の下降部分
        rb2d.velocity = new Vector2(_groundModel.XSpeed * _groundModel.Direction, -ySpeed);
        await WaitAction(_groundModel.MoveTime / 2);
        //一定時間停止。停止時間をランダムにして他のスライムと同じ動きにならないようにする
        rb2d.velocity = Vector2.zero;
        float waitTime = UnityEngine.Random.Range(0.1f, 0.5f);
        await WaitAction(0.3f + waitTime);
        stateCon.DeleteState(EnemyState.Moving);
    }

    protected override void CheckGround()
    {
        //地面と接しているとき
        if (raycaster.GetGrounded())
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

    protected override void CheckNextGround()
    {
        if (stateCon.HasState(EnemyState.Moving)
            || !stateCon.HasState(EnemyState.Standing)) return;

        if(raycaster.GetNextGrounded())
        {
            return;
        }

        //何もなかったとき
        _groundModel.Direction *= -1;
    }

    protected override void CheckWallToTurn()
    {
        if (stateCon.HasState(EnemyState.Moving)
            || !stateCon.HasState(EnemyState.Standing)) return;

        if(raycaster.GetWalled())
        {
            _groundModel.Direction *= -1;
        }

        //壁がないときは何もしない
    }

    protected override void Fall()
    {
        rb2d.velocity = new Vector2(0, _groundModel.Gravity);
    }

    protected override void SetAttack()
    {
        _groundModel.Attack = stateCon.HasState(EnemyState.Moving) ? 25 : 20;
    }

    //スライムのParalysisは時間を自由にすると移動中に速度が変わり変な挙動になるため固定
    public async override UniTask Paralysis(float _)
    {
        if (stateCon.HasState(EnemyState.Paralysised)) return;

        //計算と元に戻す用の数値
        var beforeXSpeed = _groundModel.XSpeed;
        var beforeMoveTime = _groundModel.MoveTime;
        var beforeYSpeed = ySpeed;

        stateCon.AddState(EnemyState.Paralysised);
        //動きの停止
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        Fall();
        //速さの低下
        _groundModel.XSpeed -= 1f;
        _groundModel.MoveTime = beforeXSpeed * beforeMoveTime / _groundModel.XSpeed;
        ySpeed = beforeYSpeed * beforeMoveTime / _groundModel.MoveTime;
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
        _groundModel.XSpeed = beforeXSpeed;
        _groundModel.MoveTime = beforeMoveTime;
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
