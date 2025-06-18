using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

public class Ghost : EnemyPresenter
{
    private Vector3 firstPosition; //初期位置の記録をする変数

    public override void ManualStart()
    {
        base.ManualStart();
        //初期位置の記録
        firstPosition = transform.position;
    }

    public override void ManualFixedUpdate()
    {
        SetAttack();
        Move();
    }

    private void Move()
    {
        if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
            || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
            || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
            || Model.HitPoint == 0) return;

        Model.CurrentState |= EnemyControlState.Moving;

        //xSpeed、ySpeedを足した座標へ移動
        Tween = transform.DOMove(firstPosition + new Vector3(Model.XSpeed * Model.Direction, Model.YSpeed, 0), Model.MoveSpeed, false)
            //移動が終わったら待機して、元の位置に戻る
            .OnComplete(async () =>
            {
                var token = this.GetCancellationTokenOnDestroy();
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                Tween = transform.DOMove(firstPosition, Model.MoveSpeed, false)
                //元の位置に戻ったら少し待機
                .OnComplete(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                    Model.CurrentState &= ~EnemyControlState.Moving;
                });
            });
    }

    //Attackの数値設定
    private void SetAttack()
    {
        Model.Attack = (Model.CurrentState.HasFlag(EnemyControlState.Moving)) ? 20 : 10;
    }

    public override async void StopOrder(float stopTime)
    {
        //アニメーションを停止
        Tween.Pause();
        await UniTask.WaitUntil(() => !Model.CurrentState.HasFlag(EnemyControlState.Stopped)
        , cancellationToken: this.GetCancellationTokenOnDestroy());
        //Stoppedが解除されたらアニメーションの続きを開始
        Tween.Play();
    }
}
