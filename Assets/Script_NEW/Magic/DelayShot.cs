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
            //空中で待機
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);
            //移動開始時に音を鳴らす
            AudioManager.Instance.PlaySE(AudioType.delayShot);
            //速度の設定
            rb.velocity = new Vector2(speed, 0);
            //指定時間が経ったらDestroy
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
            //敵に当たったら無くなる
            Destroy(gameObject, 0.1f);
        }
    }
}
