using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace WizardMagic
{
    public class RapidShot : Magic
    {
        private Rigidbody2D rb;

        protected override async void Action(float speed, float disappearTime)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(speed, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(disappearTime));
            Destroy(gameObject);
        }

        protected override void Effect(Enemy enemy)
        {
            Destroy(gameObject, 0.1f);
        }

        protected override void Buff(Player player)
        {
            return;
        }
    }
}
