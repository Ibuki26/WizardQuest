using UnityEngine;
using Cysharp.Threading.Tasks;

public class CactusNeedle : MonoBehaviour
{
    private int strength = 40;
    private int attack = 30;

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    //ダメージ処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent<WizardPresenter>(out var player))
        {
            player.DamageFromEnemy(attack, strength).Forget();
            Destroy(gameObject, 0.1f);
        }

        if (collision.gameObject.TryGetComponent<ShotMagic>(out _))
        {
            Destroy(gameObject, 0.1f);
        }
    }

    //画面外にでたら削除
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
