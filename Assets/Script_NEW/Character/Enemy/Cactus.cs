using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using WizardPlayer;

namespace WizardEnemy
{
    public class Cactus : EnemyPresenter
    {
        [SerializeField] private CactusNeedle needle;
        [SerializeField] private WizardPresenter player;
        [SerializeField] private Sprite[] sprites;
        private EnemySightChecker esc;
        private SpriteRenderer sr;
        private int shotCount = 0;

        public override void ManualStart()
        {
            base.ManualStart();
            esc = GetComponent<EnemySightChecker>();
            sr = GetComponent<SpriteRenderer>();
            Model.Attack = 20;
        }

        public override void ManualFixedUpdate()
        {
            //�v���C���[�Ƃ̈ʒu�̔�r
            CheckPlayer();
            //�v���C���[�����E�ɓ����Ă��邩�̊m�F
            esc.IsVisible(this);
            //�g�Q�̔���
            ShotNeedle().Forget();
        }

        private async UniTask ShotNeedle()
        {
            if (Model.CurrentState.HasFlag(EnemyControlState.Moving)
                || !Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || !Model.CurrentState.HasFlag(EnemyControlState.FindPlayer)
                || Model.HitPoint == 0) return;

            Model.CurrentState |= EnemyControlState.Moving;
            if (shotCount != 3)
            {
                shotCount++;
                AudioManager.Instance.PlaySE(AudioType.needle);
                needle.CreateNeedle(player.transform.position, this);
                sr.sprite = sprites[shotCount];
            }
            else
            {
                shotCount = 0;
                sr.sprite = sprites[0];
            }
            await UniTask.Delay(TimeSpan.FromSeconds(2f),
                cancellationToken : this.GetCancellationTokenOnDestroy());
            Model.CurrentState &= ~EnemyControlState.Moving;
        }

        private void CheckPlayer()
        {
            if (!Model.CurrentState.HasFlag(EnemyControlState.OnCamera)
                || Model.CurrentState.HasFlag(EnemyControlState.Stopped)
                || Model.HitPoint == 0) return;

            //�v���C���[�������ɂ���Ƃ�
            if(transform.position.x > player.transform.position.x)
            {
                Model.Direction = -1;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            //�v���C���[���E���ɂ���Ƃ�
            else if (transform.position.x < player.transform.position.x)
            {
                Model.Direction = 1;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        public override void StopOrder(float stopTime)
        {
            return;
        }
    }
}
