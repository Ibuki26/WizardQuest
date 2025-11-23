using UnityEngine;
using Cysharp.Threading.Tasks;

public class BuffEffecter : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;

    //InGameクラスのStartで呼び出す
    public void ManualStart()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    //AnimatorのControllerの登録
    public void SetAnimatiorController(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
    }

    //Animatorの指定したパラメーターの値変更
    public void SetAnimation(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }

    //画像の向きを変える関数
    public void SetSpriteFlip(int direction)
    {
        var flipValue = (direction > 0) ? false : true;
        sr.flipX = flipValue;
    }

    //指定したアニメーションが終わるまで待機する関数
    public async UniTask WaitAnimation(string animationName)
    {
        await UniTask.WaitUntil(() =>
           animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)
           && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
           , cancellationToken: this.GetCancellationTokenOnDestroy());
    }
}
