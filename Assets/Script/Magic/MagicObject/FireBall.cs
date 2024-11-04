using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MagicObject
{
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.velocity = new Vector2(GetSpeed(), 0);
        AudioManager.Instance.PlaySE(AudioType.fireBall);
    }

    public override void Effect(Enemy enemy)
    {
        //ノックバックとノックバックを機能させるためにEnemyを一瞬止める
        SetNotBreak(true);
        anim.SetBool("disappear", true);
        enemy.StopEnemy(0.2f);
        enemy.rb.AddForce(new Vector2(GetSpeed()/3, enemy.rb.velocity.y), ForceMode2D.Impulse);
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 0.1f);
    }
}
