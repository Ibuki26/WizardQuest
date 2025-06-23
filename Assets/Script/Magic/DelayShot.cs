using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using ShotMagicMethod;

public class DelayShot : ShotMagic, IShotMagicEffect
{
    private float waitTime = 1.0f;

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
        var rb = GetComponent<Rigidbody2D>();
        //���ԍ���_status.Direction���g���Ƒҋ@�I����̃v���C���[�̌������i�s�����ɂȂ��Ă��܂����߁A�����ŕێ�
        var direction = _model.Direction; 
        //�󒆂őҋ@
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        //�ړ��J�n���ɉ���炷
        AudioManager.Instance.PlaySE(AudioType.delayShot);
        //���x�̐ݒ�
        rb.velocity = new Vector2(_status.Speed * direction, 0);
        //�w�莞�Ԃ��o������Destroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime),
                cancellationToken: this.GetCancellationTokenOnDestroy());
        Destroy(gameObject);
    }
}
