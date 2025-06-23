using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using ShotMagicMethod;

public class RapidShot : ShotMagic, IShotMagicEffect
{
    public override void Action()
    {
        SetActionAsync().Forget();
    }

    public void Effect(EnemyModel enemyModel)
    {
        //�G�ɓ��������疳���Ȃ�
        Destroy(gameObject, 0.1f);
    }

    //�߂�l��void�̊֐�����await���邽�߂̊֐�
    private async UniTask SetActionAsync()
    {
        //���x�̐ݒ�
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(_status.Speed * _model.Direction, 0);
        //�w�莞�Ԃ��o������Destroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
