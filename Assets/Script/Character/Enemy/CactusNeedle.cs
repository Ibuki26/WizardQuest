using UnityEngine;
using Cysharp.Threading.Tasks;

public class CactusNeedle : MonoBehaviour
{
    private int strength;
    private int attack;

    //�^�[�Q�b�g���󂯎���Đ��������֐�
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
        //target�ւ̃x�N�g�����v�Z���A������1�ɒ��߂��Ă���傫����ύX
        return new Vector2(target.x - position.x, target.y - position.y).normalized * 3f;
    }

    //�_���[�W����
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

    //��ʊO�ɂł���폜
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
