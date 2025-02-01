using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using WizardPlayer;

namespace WizardEnemy
{
    public class Slime : EnemyPresenter
    {
        private RaycastHit2D standHit,hit,hit2;

        private float adjustRaycastWall_x = 0.45f;
        private float adjustRaycastGround_y = 0.27f;
        private float gravity = -1.0f;

        public override void ManualFixedUpdate()
        {
            IsGround();
            CheckJumpToGround();
            CheckWall();
            AdjustVelocity_y();
            SetAttck();
            Move();
        }

        private void Move()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
                || !Model.CurrentState.HasFlag(EnemyControlState.Standing)
                || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            Model.CurrentState |= EnemyControlState.Moving;
            //�ړ����鋗���̌v�Z �W�����v���Ė߂邽��XSpeed��2�{����
            var movedPosition_x = Model.XSpeed * Model.Direction * 2;
            Tween = transform.DOJump(transform.position + new Vector3(movedPosition_x, 0, 0), Model.YSpeed, 1, Model.MoveSpeed, false)
                //DoTween�A�j���[�V�����I����ɌĂяo��
                .OnComplete(async () =>
                {
                    var token = this.GetCancellationTokenOnDestroy();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: token);
                    Model.CurrentState &= ~EnemyControlState.Moving;
                });
        }

        public override void StopOrder(float stopTime)
        {
            Tween.Kill();
        }

        public void SetAttck()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving))
                Model.Attack = 20;
            else
                Model.Attack = 10;
        }

        public void AdjustVelocity_y()
        {
            //Die�̏����Ɣ��\�������邽�߁B�̗͂�0�̂Ƃ��͎��s���Ȃ�
            if (Model.HitPoint == 0) return;

            //�ړ����ł͂Ȃ��A�ڒn���Ă��Ȃ��Ƃ����x��-4�ɂ��ė��Ƃ�
            if (!Model.CurrentState.HasFlag(EnemyControlState.Moving)
                && !Model.CurrentState.HasFlag(EnemyControlState.Standing))
            {
                RB.velocity = new Vector2(0, gravity);
            }

            if (Model.CurrentState.HasFlag(EnemyControlState.Standing) && RB.velocity.y < 0)
            {
                RB.velocity = Vector2.zero;
            }
        }

        //�n�ʂƐڂ��Ă��邩�̔���
        private void IsGround()
        {
            standHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycastGround_y), Vector2.down, 0.01f);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycastGround_y), Vector2.down * 0.01f, Color.red);
            //�n�ʂƐڂ��Ă���Ƃ�
            if (standHit.collider != null && !standHit.collider.isTrigger)
            {
                Model.CurrentState |= EnemyControlState.Standing;
                return;
            }

            Model.CurrentState &= ~EnemyControlState.Standing;
            return;
        }

        //�ړ���ɒn�ʂ����邩�̊m�F
        private void CheckJumpToGround()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
                || !Model.CurrentState.HasFlag(EnemyControlState.Standing)
                || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            //Raycast�̔����ʒu�̒���
            var adjustPosition_x = transform.position.x + Model.XSpeed * 2 * Model.Direction;
            hit = Physics2D.Raycast(new Vector2(adjustPosition_x, transform.position.y - adjustRaycastGround_y), Vector2.down, 0.05f);
            Debug.DrawRay(new Vector2(adjustPosition_x , transform.position.y - adjustRaycastGround_y), Vector2.down * 0.05f, Color.red);
            //�n�ʂ��������Ƃ�
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                return;
            }

            //�����Ȃ������Ƃ�
            Model.Direction *= -1;
        }

        //�ړ������ɕǂ����邩�̊m�F
        private void CheckWall()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
                || !Model.CurrentState.HasFlag(EnemyControlState.Standing)
                || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            //Raycast�̔����ʒu�̒���
            var adjustPosition_x = transform.position.x + adjustRaycastWall_x * Model.Direction;
            var direction = new Vector2(Model.Direction, 0);
            hit2 = Physics2D.Raycast(new Vector2(adjustPosition_x, transform.position.y), direction, Model.XSpeed * 2 + 0.4f);
            Debug.DrawRay(new Vector2(adjustPosition_x, transform.position.y), direction * (Model.XSpeed * 2 + 0.4f), Color.red);
            //���I�u�W�F�N�g�ɂԂ������Ƃ�
            if(hit2.collider != null && !hit2.collider.isTrigger)
            {
                //�Ԃ������I�u�W�F�N�g����Enemy��Player�̂Ƃ��͖���
                if(hit2.collider.TryGetComponent<EnemyPresenter>(out _) ||
                    hit2.collider.TryGetComponent<WizardPresenter>(out _))
                {
                    return;
                }

                Model.Direction *= -1;
            }
        }
    }
}
