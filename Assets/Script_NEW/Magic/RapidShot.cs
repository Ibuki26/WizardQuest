using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using WizardEnemy;

namespace WizardMagic
{
    public class RapidShot : ShotMagic
    {
        private Rigidbody2D rb;

        protected override async void Action(float speed, float destroyTime)
        {
            //ë¨ìxÇÃê›íË
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(speed, 0);
            var token = this.GetCancellationTokenOnDestroy();
            //éwíËéûä‘Ç™åoÇ¡ÇΩÇÁDestroy
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
            //ìGÇ…ìñÇΩÇ¡ÇΩÇÁñ≥Ç≠Ç»ÇÈ
            Destroy(gameObject, 0.1f);
        }
    }
}
