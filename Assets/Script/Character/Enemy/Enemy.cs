using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hitPoint;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private int attack;
    [SerializeField] int score;
    [SerializeField] private Color damageColor; //�_���[�W���󂯂��Ƃ��̐F
    [SerializeField] private UIManager ui;
    public Rigidbody2D rb;
    private Collider2D coll;
    private Vector2 enemyVelocity;
    protected SpriteRenderer sr;
    private bool ignoreDamage = false; //�_���[�W���󂯂Ȃ��Ƃ���true
    private bool onCamera = false;
    private bool stoped = false; //�O������̒�~���߂��󂯂�ϐ�

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<MagicObject>(out var magicObject))
        {
            Damage(magicObject.GetAttack());
            magicObject.Effect(gameObject.GetComponent<Enemy>());
        }

        if (collision.transform.parent != null && collision.transform.parent.TryGetComponent<Player>(out var player))
        {
            player.Damage(attack);
        }
    }

    private void OnBecameVisible()
    {
        onCamera = true;
    }

    private void OnBecameInvisible()
    {
        onCamera = false;
    }

    //�_���[�W����
    private async void Damage(int attack)
    {
        if (ignoreDamage) return;

        ignoreDamage = true;

        AudioManager.Instance.PlaySE(AudioType.damage_moster);
        //�_���[�W���󂯂��u�ԐF��ς���
        sr.color = damageColor;
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        sr.color = Color.white;

        //hitPoint�̌v�Z
        if (hitPoint - attack <= 0)
        {
            hitPoint = 0;
            Died();

            return;
        }
        else
            hitPoint = hitPoint - attack;

        ignoreDamage = false;
    }

    public void Died()
    {
        if (hitPoint > 0) return;

        //���ꂽ���𗬂��A�X�R�A�ɉ��Z
        AudioManager.Instance.PlaySE(AudioType.die_moster);
        coll.enabled = false;
        SetVelocity_x(0);
        SetVelocity_y(-2f);
        rb.velocity = enemyVelocity;
        ui.AddScore(score);
        Destroy(gameObject, 2f);
    }

    public async void StopEnemy(float time)
    {
        stoped = true;
        rb.velocity = new Vector2(0, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        stoped = false;
    }

    public int GetHitPoint()
    {
        return hitPoint;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetJump()
    {
        return jump;
    }

    public int GetAttack()
    {
        return attack;
    }

    public Vector2 GetEnemyVelocity()
    {
        return enemyVelocity;
    }

    public int GetScore()
    {
        return score;
    }
    
    public bool GetOnCamera()
    {
        return onCamera;
    }

    public bool GetStoped()
    {
        return stoped;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetJump(float j)
    {
        jump = j;
    }

    public void SetVelocity_x(float x)
    {
        enemyVelocity.x = x;
    }

    public void SetVelocity_y(float y)
    {
        enemyVelocity.y = y;
    }

    public void SetStoped(bool b)
    {
        stoped = b;
    }
}
