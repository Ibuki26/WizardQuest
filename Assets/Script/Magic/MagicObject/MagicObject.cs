using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicObject : MonoBehaviour
{
    [SerializeField] private int attack;
    [SerializeField] private float speed;
    [SerializeField] private float disappearTime;
    private float time = 0;
    private bool notBreak = false;

    public abstract void Effect(Enemy enemy);

    void Update()
    {
        if (time > disappearTime)
        {
            if(!notBreak)
                Destroy(gameObject);
        }

        time += Time.deltaTime;
    }

    public int GetAttack()
    {
        return attack;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetDisappearTime()
    {
        return disappearTime;
    }

    public void SetAttack(int a)
    {
        attack = a;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetDisappearTime(float dt)
    {
        disappearTime = dt;
    }

    public void SetNotBreak(bool b)
    {
        notBreak = b;
    }
}
