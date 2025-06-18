using UnityEngine;
using Cysharp.Threading.Tasks;

public class FireMeteor : ShotMagic
{
    private Rigidbody2D rb;
    private Animator anim;

    private float speed_y = -3.0f;

    protected override void Action(float speed, float destroyTime)
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //���x�̐ݒ�
        rb.velocity = new Vector2(speed, speed_y);
    }

    public async override void Effect(EnemyPresenter enemy)
    {
        //�G�ɓ��������Ƃ��A�A�j���[�V�������J�n���������~�߂�
        anim.SetBool("hit", true);
        rb.velocity = Vector2.zero;

        //�A�j���[�V�������I���܂őҋ@
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("Fire_Meteors_Large_A_End")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }

    //��ʊO�ɏo���Ƃ�Destroy
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
