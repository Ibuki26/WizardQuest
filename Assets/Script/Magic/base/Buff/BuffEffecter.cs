using UnityEngine;
using Cysharp.Threading.Tasks;

public class BuffEffecter : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    //InGame�N���X��Start�ŌĂяo��
    public void ManualStart()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    //Animator��Controller�̓o�^
    public void SetAnimatiorController(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
    }

    //Animator�̎w�肵���p�����[�^�[�̒l�ύX
    public void SetAnimation(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }

    //�摜�̌�����ς���֐�
    public void SetSpriteFlip(int direction)
    {
        var flipValue = (direction > 0) ? false : true;
        sr.flipX = flipValue;
    }

    //�w�肵���A�j���[�V�������I���܂őҋ@����֐�
    public async UniTask WaitAnimation(string animationName)
    {
        await UniTask.WaitUntil(() =>
           animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)
           && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
           , cancellationToken: this.GetCancellationTokenOnDestroy());
    }
}
