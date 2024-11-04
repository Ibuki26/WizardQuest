using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidShot : MagicObject
{
    private Rigidbody2D rb;

    void Start()
    {
        AudioManager.Instance.PlaySE(AudioType.rapidShot);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(GetSpeed(), 0);
    }

    public override void Effect(Enemy enemy)
    {
        Destroy(gameObject, 0.1f);
    }
}
