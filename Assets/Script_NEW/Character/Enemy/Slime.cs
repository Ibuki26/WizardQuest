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
            //移動する距離の計算 ジャンプして戻るためXSpeedを2倍する
            var movedPosition_x = Model.XSpeed * Model.Direction * 2;
            Tween = transform.DOJump(transform.position + new Vector3(movedPosition_x, 0, 0), Model.YSpeed, 1, Model.MoveSpeed, false)
                //DoTweenアニメーション終了後に呼び出し
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
            //Dieの処理と被る可能性があるため。体力が0のときは実行しない
            if (Model.HitPoint == 0) return;

            //移動中ではなく、接地していないとき速度を-4にして落とす
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

        //地面と接しているかの判定
        private void IsGround()
        {
            standHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycastGround_y), Vector2.down, 0.01f);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycastGround_y), Vector2.down * 0.01f, Color.red);
            //地面と接しているとき
            if (standHit.collider != null && !standHit.collider.isTrigger)
            {
                Model.CurrentState |= EnemyControlState.Standing;
                return;
            }

            Model.CurrentState &= ~EnemyControlState.Standing;
            return;
        }

        //移動先に地面があるかの確認
        private void CheckJumpToGround()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
                || !Model.CurrentState.HasFlag(EnemyControlState.Standing)
                || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            //Raycastの発生位置の調整
            var adjustPosition_x = transform.position.x + Model.XSpeed * 2 * Model.Direction;
            hit = Physics2D.Raycast(new Vector2(adjustPosition_x, transform.position.y - adjustRaycastGround_y), Vector2.down, 0.05f);
            Debug.DrawRay(new Vector2(adjustPosition_x , transform.position.y - adjustRaycastGround_y), Vector2.down * 0.05f, Color.red);
            //地面があったとき
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                return;
            }

            //何もなかったとき
            Model.Direction *= -1;
        }

        //移動方向に壁があるかの確認
        private void CheckWall()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
                || !Model.CurrentState.HasFlag(EnemyControlState.Standing)
                || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            //Raycastの発生位置の調整
            var adjustPosition_x = transform.position.x + adjustRaycastWall_x * Model.Direction;
            var direction = new Vector2(Model.Direction, 0);
            hit2 = Physics2D.Raycast(new Vector2(adjustPosition_x, transform.position.y), direction, Model.XSpeed * 2 + 0.4f);
            Debug.DrawRay(new Vector2(adjustPosition_x, transform.position.y), direction * (Model.XSpeed * 2 + 0.4f), Color.red);
            //他オブジェクトにぶつかったとき
            if(hit2.collider != null && !hit2.collider.isTrigger)
            {
                //ぶつかったオブジェクトが他EnemyかPlayerのときは無視
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
