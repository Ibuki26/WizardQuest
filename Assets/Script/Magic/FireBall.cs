using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class FireBall : ShotMagic
{
    private Rigidbody2D rb;
    private Animator anim;
    private CancellationTokenSource cts;

    protected async override void Action(float speed, float destroyTime)
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

    public override async void Effect(EnemyPresenter enemy)
    {
        //Action��await���L�����Z��
        cts.Cancel();
        anim.SetBool("disappear", true);

        //�m�b�N�o�b�N�������
        int direction = (rb.velocity.x > 0) ? 1 : -1;
        //Enemy���m�b�N�o�b�N����
        enemy.KnockBack(direction * 0.8f).Forget();

        rb.velocity = Vector2.zero;

        //�A�j���[�V�������I���܂őҋ@
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("FireBall_disappear")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }
}
