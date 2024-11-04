using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField] private int attack;

    //�^�[�Q�b�g���󂯎���Đ��������֐�
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
        //target�ւ̃x�N�g�����v�Z���A������1�ɒ��߂��Ă���傫����ύX
        return new Vector2(target.x - position.x, target.y - position.y).normalized * 3f;
    }

    //�_���[�W����
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

    //��ʊO�ɂł���폜
    private void OnBecameInvisible()
    {
        Destroy(gameObject, 1f);
    }
}
