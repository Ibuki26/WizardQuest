using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class RapidShot : ShotMagic
{
    private Rigidbody2D rb;

    protected override async void Action(float speed, float destroyTime)
    {
        //‘¬“x‚Ìİ’è
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        var token = this.GetCancellationTokenOnDestroy();
        //w’èŠÔ‚ªŒo‚Á‚½‚çDestroy
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
        //“G‚É“–‚½‚Á‚½‚ç–³‚­‚È‚é
        Destroy(gameObject, 0.1f);
    }
}
