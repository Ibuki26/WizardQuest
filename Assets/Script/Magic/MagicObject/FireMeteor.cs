using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeteor : MagicObject
{
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        AudioManager.Instance.PlaySE(AudioType.fireMeteor);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2(GetSpeed(), -3f);
    }

    public override void Effect(Enemy enemy)
    {
        SetNotBreak(true);
        anim.SetBool("hit", true);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 0.2f);
    }
}
