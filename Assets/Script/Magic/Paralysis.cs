using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class Paralysis : ShotMagic
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;

    protected override async void Action(float speed, float destroyTime)
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cts = new CancellationTokenSource();
        //���x�̐ݒ�
        rb.velocity = new Vector2(speed, 0);
        //�w�莞�Ԃ��o������Destroy
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(destroyTime), cancellationToken: cts.Token);
            Destroy(gameObject);
        }
        catch
        {
            //Debug.Log("Action��Destroy���L�����Z������܂����B");
        }
    }

    public async override void Effect(EnemyPresenter enemy)
    {
        //Action��await���L�����Z��
        cts.Cancel();

        //�����Đ��ƃA�j���[�V�����̍Đ�
        AudioManager.Instance.PlaySE(AudioType.paralysis);
        rb.velocity = Vector2.zero;
        anim.SetBool("hit", true);

        //Enemy�̓������~�߂�
        enemy.Stop(3.0f).Forget();

        //�A�j���[�V�������I���܂őҋ@
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("Paralysis_hit")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }
}
