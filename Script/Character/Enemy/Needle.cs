using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] private int attack;

    //ターゲットを受け取って生成される関数
    public void CreateNeedle(Vector2 target, Vector3 position)
    {
        var myVelocity = CalculateVelocity(target, new Vector2(position.x, position.y));
        var angle = Mathf.Atan2(myVelocity.x, myVelocity.y);
        var needle = Instantiate(gameObject, position, Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg));
        var rb = needle.GetComponent<Rigidbody2D>();
        rb.velocity = myVelocity;
    }

    private Vector2 CalculateVelocity(Vector2 target, Vector2 position)
    {
        //targetへのベクトルを計算し、長さを1に調節してから大きさを変更
        return new Vector2(target.x - position.x, target.y - position.y).normalized * 3f;
    }

    //ダメージ処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent<Player>(out var player))
        {
            player.Damage(attack);
            Destroy(gameObject, 0.1f);
        }

        if (collision.gameObject.TryGetComponent<MagicObject>(out var magicObject))
        {
            Destroy(gameObject, 0.1f);
        }
    }

    //画面外にでたら削除
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 1f);
    }
}
