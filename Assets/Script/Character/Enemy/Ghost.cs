using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

public class Ghost : EnemyPresenter
{
    private Vector3 firstPosition; //�����ʒu�̋L�^������ϐ�

    public override void ManualStart()
    {
        base.ManualStart();
        //�����ʒu�̋L�^
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

        //xSpeed�AySpeed�𑫂������W�ֈړ�
        Tween = transform.DOMove(firstPosition + new Vector3(Model.XSpeed * Model.Direction, Model.YSpeed, 0), Model.MoveSpeed, false)
            //�ړ����I�������ҋ@���āA���̈ʒu�ɖ߂�
            .OnComplete(async () =>
            {
                var token = this.GetCancellationTokenOnDestroy();
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                Tween = transform.DOMove(firstPosition, Model.MoveSpeed, false)
                //���̈ʒu�ɖ߂����班���ҋ@
                .OnComplete(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
                    Model.CurrentState &= ~EnemyControlState.Moving;
                });
            });
    }

    //Attack�̐��l�ݒ�
    private void SetAttack()
    {
        Model.Attack = (Model.CurrentState.HasFlag(EnemyControlState.Moving)) ? 20 : 10;
    }

    public override async void StopOrder(float stopTime)
    {
        //�A�j���[�V�������~
        Tween.Pause();
        await UniTask.WaitUntil(() => !Model.CurrentState.HasFlag(EnemyControlState.Stopped)
        , cancellationToken: this.GetCancellationTokenOnDestroy());
        //Stopped���������ꂽ��A�j���[�V�����̑������J�n
        Tween.Play();
    }
}
