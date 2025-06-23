using UnityEngine;
using Cysharp.Threading.Tasks;
using ShotMagicMethod;

public class FireMeteor : ShotMagic, IShotMagicEffect
{
    private Rigidbody2D rb;
    private Animator anim;

    private float speed_y = -3.0f;

    public override void Action()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //���x�̐ݒ�
        rb.velocity = new Vector2(_status.Speed * _model.Direction, speed_y);
    }

    public void Effect(EnemyModel enemyModel)
    {
        SetEffectAsync(enemyModel).Forget();
    }

    //��ʊO�ɏo���Ƃ�Destroy
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //�߂�l��void�̊֐�����await���邽�߂̊֐�
    private async UniTask SetEffectAsync(EnemyModel enemyModel)
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
}
