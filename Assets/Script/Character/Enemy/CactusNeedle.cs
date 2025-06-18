using UnityEngine;
using Cysharp.Threading.Tasks;

public class CactusNeedle : MonoBehaviour
{
    private int strength;
    private int attack;

    //ターゲットを受け取って生成される関数
    public void CreateNeedle(Vector2 target, EnemyPresenter enemy)
    {
        var myVelocity = CalculateVelocity(target, new Vector2(enemy.transform.position.x, enemy.transform.position.y));
        var angle = Mathf.Atan2(myVelocity.x, myVelocity.y);
        var needle = Instantiate(gameObject, enemy.transform.position, Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg));
        var rb = needle.GetComponent<Rigidbody2D>();
        rb.velocity = myVelocity;
        var cactusNeedle = needle.GetComponent<CactusNeedle>();
        cactusNeedle.strength = enemy.Model.Strength;
        cactusNeedle.attack = enemy.Model.Attack;
    }

    private Vector2 CalculateVelocity(Vector2 target, Vector2 position)
    {
        //targetへのベクトルを計算し、長さを1に調節してから大きさを変更
        return new Vector2(target.x - position.x, target.y - position.y).normalized * 3f;
    }

    //ダメージ処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent<WizardPresenter>(out var player))
        {
            player.DamageFromEnemy(attack, strength).Forget();
            Destroy(gameObject, 0.1f);
        }

        if (collision.gameObject.TryGetComponent<Magic>(out _))
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
