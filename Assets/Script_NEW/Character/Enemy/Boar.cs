using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardPlayer;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;

namespace WizardEnemy
{
    public class Boar : EnemyPresenter
    {
        private RaycastHit2D hit, hit2;
        private EnemySightChecker esc;

        private float adjustRaycast_x = 0.65f;
        private float adjustRaycastGround_y = 0.6f;

        public override void ManualStart()
        {
            base.ManualStart();
            esc = GetComponent<EnemySightChecker>();
        }

        public override void ManualFixedUpdate()
        {
            CheckMoveToGround();
            CheckWall();
            esc.IsVisible(this);
            CheckSight();
            SetAttack();
            Move();
        }

        private void Move()
        {
            if (!Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                 || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                 || Model.HitPoint == 0) return;

            Tween = transform.DOMove(transform.position + new Vector3(Model.XSpeed * Model.Direction, 0, 0)
                , Model.MoveSpeed, false);
        }

        //Attack�̐��l�ݒ�
        private void SetAttack()
        {
            Model.Attack = Model.CurrentState.HasFlag(EnemyControlState.Moving) ? 50 : 30;
        }

        public override void StopOrder(float stopTime)
        {
            Tween.Kill();
        }

        private async void CheckSight()
        {
            if (!Model.CurrentState.HasFlag(EnemyControlState.Finding)
                && Model.CurrentState.HasFlag(EnemyControlState.FindPlayer))
            {
                Model.CurrentState |= EnemyControlState.Finding;
                Model.MoveSpeed -= 0.15f;
                await UniTask.WaitUntil(() => !Model.CurrentState.HasFlag(EnemyControlState.FindPlayer));
                await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
                Model.CurrentState &= ~EnemyControlState.Finding;
                Model.MoveSpeed += 0.15f;
            }
        }

        //�ړ���ɒn�ʂ����邩�̊m�F
        private void CheckMoveToGround()
        {
            if (!Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            hit = Physics2D.Raycast(new Vector2(transform.position.x + adjustRaycast_x * Model.Direction, transform.position.y - adjustRaycastGround_y), Vector2.down, 0.1f);
            Debug.DrawRay(new Vector2(transform.position.x + adjustRaycast_x * Model.Direction, transform.position.y - adjustRaycastGround_y), Vector2.down * 0.1f, Color.red);
            //�n�ʂƐڂ��Ă���Ƃ�
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                return;
            }

            Model.Direction *= -1;
            //�����̒���
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Model.Direction, transform.localScale.y, transform.localScale.z);
            return;
        }

        //�ړ������ɕǂ����邩�̊m�F
        private void CheckWall()
        {
            if (!Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            //Raycast�̔����ʒu�̒���
            var adjustPosition_x = transform.position.x + adjustRaycast_x * Model.Direction;
            var direction = new Vector2(Model.Direction, 0);
            hit2 = Physics2D.Raycast(new Vector2(adjustPosition_x, transform.position.y), direction, 0.1f);
            Debug.DrawRay(new Vector2(adjustPosition_x, transform.position.y), direction * 0.1f, Color.red);
            //���I�u�W�F�N�g�ɂԂ������Ƃ�
            if (hit2.collider != null && !hit2.collider.isTrigger)
            {
                //�Ԃ������I�u�W�F�N�g����Enemy��Player�̂Ƃ��͖���
                if (hit2.collider.TryGetComponent<EnemyPresenter>(out _) ||
                    hit2.collider.TryGetComponent<WizardPresenter>(out _))
                {
                    return;
                }

                Model.Direction *= -1;
                //�����̒���
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Model.Direction, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
