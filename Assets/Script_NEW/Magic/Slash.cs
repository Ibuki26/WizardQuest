using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace WizardMagic
{
    public class Slash : Magic
    {
        protected override async void Action(float speed, float disappearTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(disappearTime));
            Destroy(gameObject);
        }

        protected override void Effect(Enemy enemy)
        {
            return;
        }

        protected override void Buff(Player player)
        {
            return;
        }
    }
}
