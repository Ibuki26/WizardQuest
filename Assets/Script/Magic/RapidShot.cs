using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class RapidShot : ShotMagic
{
    private Rigidbody2D rb;

    protected override async void Action(float speed, float destroyTime)
    {
        //���x�̐ݒ�
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
        var token = this.GetCancellationTokenOnDestroy();
        //�w�莞�Ԃ��o������Destroy
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
        //�G�ɓ��������疳���Ȃ�
        Destroy(gameObject, 0.1f);
    }
}
