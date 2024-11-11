using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public class FireBall : Magic
    {
        private Rigidbody2D rb;
        private Animator anim;

        protected override void Action(float speed, float disappearTime)
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            rb.velocity = new Vector2(speed, 0);
        }
        
        protected override void Effect(Enemy enemy)
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Buff(Player player)
        {
            return;
        }
    }
}
