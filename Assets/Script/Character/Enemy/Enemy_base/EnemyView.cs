using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Color damageColor; //ダメージを受けたときに変わる色
    private SpriteRenderer sr;

    public void ManualStart()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    //ダメージを受けた時に指定した色に変えて、0.2秒後元に戻す
    public async UniTask DamageColor()
    {
        sr.color = damageColor;
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        sr.color = Color.white;
    }
}
