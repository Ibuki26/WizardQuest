using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using ShotMagicMethod;

public class Paralysis : ShotMagic, IShotMagicEffect
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;
    private bool hasDispose = false; //Effect��ɓ�d��Dispose������̂�������邽��

    public override void Action()
    {
        SetActionAsync().Forget();
    }

    public void Effect(EnemyModel enemyModel)
    {
        SetEffectAsync(enemyModel).Forget();
    }

    //�߂�l��void�̊֐�����await���邽�߂̊֐�
    private async UniTask SetActionAsync()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cts = new CancellationTokenSource();
        //���x�̐ݒ�
        rb.velocity = new Vector2(_status.Speed * _model.Direction, 0);
        //�w�莞�Ԃ��o������Destroy
        await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime), cancellationToken: cts.Token);
        Destroy(gameObject);
    }

    //�߂�l��void�̊֐�����await���邽�߂̊֐�
    private async UniTask SetEffectAsync(EnemyModel enemyModel)
    {
        //Action��await���L�����Z��
        cts?.Cancel();
        cts?.Dispose();
        hasDispose = true;

        //�����Đ��ƃA�j���[�V�����̍Đ�
        AudioManager.Instance.PlaySE(AudioType.paralysis);
        rb.velocity = Vector2.zero;
        anim.SetBool("hit", true);

        //Enemy�̓������~�߂�
        //enemy.Stop(3.0f).Forget();

        //�A�j���[�V�������I���܂őҋ@
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("Paralysis_hit")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (!hasDispose)
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}
