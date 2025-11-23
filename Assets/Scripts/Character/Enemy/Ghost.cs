using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Ghost : EnemyPresenter
{
    [SerializeField] private float speed;
    [SerializeField] private float moveTime;
    [SerializeField] private bool onVertical;
    private Vector3 startPosition;
    private Vector3 finishPosition;
    private bool hasFinished = false;
    private Rigidbody2D rb2d;

    public override void ManualStart()
    {
        base.ManualStart();
        rb2d = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        finishPosition = SetFinishPosition();
        if(!onVertical)
            _view.FlipXImage(_model.Direction);
        else
            _view.FlipXImage(-1);
    }

    public override void ManualFixedUpdate()
    {
        //画面外にいるときと体力が0のときは実行しない
        if (!stateCon.HasState(EnemyState.OnCamera)
            || stateCon.HasState(EnemyState.Stopped)
            || _model.HitPoint == 0) return;

        SetAttack();
        Move().Forget();
    }

    protected override void SetAttack()
    {
        _model.Attack = stateCon.HasState(EnemyState.Moving) ? 30 : 25;
    }

    private async UniTask Move()
    {
        if (stateCon.HasState(EnemyState.Moving)) return;

        stateCon.AddState(EnemyState.Moving);
        //向いている方向に進む
        if (!hasFinished)
        {
            var goVector = finishPosition - transform.position;
            Vector2 myVelocity = new Vector2(goVector.x, goVector.y).normalized * speed;
            rb2d.velocity = myVelocity;
            await UniTask.WaitUntil(HasGoFinished, cancellationToken : cts.Token);
            rb2d.velocity = Vector2.zero;
            hasFinished = true;
        }
        else
        {
            var backVector = startPosition - transform.position;
            Vector2 myVelocity = new Vector2(backVector.x, backVector.y).normalized * speed;
            rb2d.velocity = myVelocity;
            await UniTask.WaitUntil(HasBackFinished, cancellationToken : cts.Token);
            rb2d.velocity = Vector2.zero;
            hasFinished = false;
        }

        if (!onVertical)
        {
            _model.Direction *= -1;
            _view.FlipXImage(_model.Direction);
        }
        await UniTask.Delay(TimeSpan.FromSeconds(1f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        stateCon.DeleteState(EnemyState.Moving);
    }

    private Vector3 SetFinishPosition()
    {
        var position = transform.position;
        if (onVertical)
        {
            //onHorizotalのときDirectionは1が上、-1が下
            var adjustPositionY = speed * moveTime * _model.Direction;
            return new Vector3(position.x, position.y + adjustPositionY, position.z);
        }
        else
        {
            var adjustPositionX = speed * moveTime * _model.Direction;
            return new Vector3(position.x + adjustPositionX, position.y, position.z);
        }
    }

    private bool HasGoFinished()
    {
        //縦移動
        if (onVertical)
        {
            //上に進むとき
            if (finishPosition.y > startPosition.y)
                return transform.position.y >= finishPosition.y;
            //下に進むとき
            else
                return transform.position.y <= finishPosition.y;
        }
        //横移動
        else
        {
            //右に進むとき
            if (finishPosition.x > startPosition.x)
                return transform.position.x >= finishPosition.x;
            //左に進むとき
            else
                return transform.position.x <= finishPosition.x;
        }
    }

    private bool HasBackFinished()
    {
        //縦移動
        if (onVertical)
        {
            //下に戻るとき
            if (finishPosition.y > startPosition.y)
                return transform.position.y <= startPosition.y;
            //上に戻るとき
            else
                return transform.position.y >= startPosition.y;
        }
        //横移動
        else
        {
            //左に戻るとき
            if (finishPosition.x > startPosition.x)
                return transform.position.x <= startPosition.x;
            //右に戻るとき
            else
                return transform.position.x >= startPosition.x;
        }
    }

    protected override void OnBecameVisible()
    {
        base.OnBecameVisible();
        rb2d.WakeUp();
    }

    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        StopAsyncTasks();
        rb2d.Sleep();
    }

    public async override UniTask Paralysis(float duration)
    {
        if (stateCon.HasState(EnemyState.Paralysised)) return;

        //計算と元に戻す用の数値
        var beforeSpeed = speed;
        var beforeMoveTime = moveTime;

        stateCon.AddState(EnemyState.Paralysised);
        //動きの停止
        stateCon.AddState(EnemyState.Stopped);
        StopAsyncTasks();
        //速さの低下
        speed -= 1.5f;
        moveTime = beforeSpeed * beforeMoveTime / speed;
        //スタン
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //動きの再開
        stateCon.DeleteState(EnemyState.Stopped);
        //麻痺が切れるまでの待機
        await UniTask.Delay(TimeSpan.FromSeconds(duration),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //速さをもとに戻す
        stateCon.DeleteState(EnemyState.Paralysised);
        speed = beforeSpeed;
        moveTime = beforeMoveTime;
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

    protected override EnemyModel CreateModel()
    {
        return _model = new EnemyModel(hp, strength, defense, score, direction);
    }
}