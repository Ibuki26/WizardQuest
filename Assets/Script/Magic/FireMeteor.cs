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
        //速度の設定
        rb.velocity = new Vector2(speed, speed_y);
    }

    public async override void Effect(EnemyPresenter enemy)
    {
        //敵に当たったとき、アニメーションを開始し動きを止める
        anim.SetBool("hit", true);
        rb.velocity = Vector2.zero;

        //アニメーションが終わるまで待機
        await UniTask.WaitUntil(() =>
            anim.GetCurrentAnimatorStateInfo(0).IsName("Fire_Meteors_Large_A_End")
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
            , cancellationToken: this.GetCancellationTokenOnDestroy());

        Destroy(gameObject);
    }

    //画面外に出たときDestroy
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
