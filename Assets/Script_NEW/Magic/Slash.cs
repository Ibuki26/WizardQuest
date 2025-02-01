using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using WizardEnemy;

namespace WizardMagic
{
    public class Slash : ShotMagic
    {
        protected override async void Action(float speed, float disappearTime)
        {
            var token = this.GetCancellationTokenOnDestroy();
            //指定時間が経ったらDestroy
            await UniTask.Delay(TimeSpan.FromSeconds(disappearTime), cancellationToken: token);
            Destroy(gameObject);
        }

        public override void Effect(EnemyPresenter enemy)
        {
            //敵に当たっても、残るため処理なし
            return;
        }
    }
}
