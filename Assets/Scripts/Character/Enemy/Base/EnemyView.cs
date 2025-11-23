using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Color damageColor; //ダメージを受けたときに変わる色
    private Animator anim;
    private SpriteRenderer sr;

    public void ManualStart()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    //ダメージを受けた時に指定した色に変えて、0.2秒後元に戻す
    public async UniTask DamageColor()
    {
        sr.color = damageColor;
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f),
            cancellationToken : this.GetCancellationTokenOnDestroy());
        sr.color = Color.white;
    }

    //指定したAnimatorのint型パラメーターの値を更新
    public void SetAnimatorInt(string name, int value)
    {
        anim.SetInteger(name, value);
    }

    //画像を反転させる関数
    public void FlipXImage(int direction)
    {
        if(direction == 0)
        {
            Debug.Log("FlipxImage関数の引数に0が代入されています");
            return;
        }

        sr.flipX = (direction > 0) ? true : false;
    }
}
