using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using WizardEnemy;

namespace WizardMagic
{
    public class DelayShot : ShotMagic
    {
        private Rigidbody2D rb;
        private float waitTime = 1.0f;

        protected async override void Action(float speed, float destroyTime)
        {
            rb = GetComponent<Rigidbody2D>();
            var token = this.GetCancellationTokenOnDestroy();
            //�󒆂őҋ@
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);
            //�ړ��J�n���ɉ���炷
            AudioManager.Instance.PlaySE(AudioType.delayShot);
            //���x�̐ݒ�
            rb.velocity = new Vector2(speed, 0);
            //�w�莞�Ԃ��o������Destroy
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(destroyTime), cancellationToken: token);
                Destroy(gameObject);
            }
            catch
            {

            }
        }

        public override void Effect(EnemyPresenter enemy)
        {
            //�G�ɓ��������疳���Ȃ�
            Destroy(gameObject, 0.1f);
        }
    }
}
