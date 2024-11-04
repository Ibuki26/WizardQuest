using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DelayShot : MagicObject
{
    private Rigidbody2D rb;

    void Start()
    {
        AudioManager.Instance.PlaySE(AudioType.magicDefalut);
        var token = this.GetCancellationTokenOnDestroy();
        SetUp(1f, token).Forget();
    }

    public override void Effect(Enemy enemy)
    {
        Destroy(gameObject, 0.1f);
    }

    private async UniTask SetUp(float time, CancellationToken token)
    {
        rb = GetComponent<Rigidbody2D>();
        await UniTask.Delay(TimeSpan.FromSeconds(time) , cancellationToken: token);
        AudioManager.Instance.PlaySE(AudioType.delayShot);
        rb.velocity = new Vector2(GetSpeed(), 0);
    }
}
