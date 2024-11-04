using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Paralysis : MagicObject
{
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        AudioManager.Instance.PlaySE(AudioType.magicDefalut);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2(GetSpeed(), 0);
    }

    public override void Effect(Enemy enemy)
    {
        AudioManager.Instance.PlaySE(AudioType.paralysis);
        SetNotBreak(true);
        enemy.StopEnemy(3.0f);
        anim.SetBool("hit", true);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 0.3f);
    }
}
