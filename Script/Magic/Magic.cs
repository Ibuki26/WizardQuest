using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private float coolTime;
    [SerializeField] private Vector3 adjustPos;
    [SerializeField] private Vector3 scale;
    public MagicObject magicObject;
    private bool coolTimeFlag = false;
    private float time = 0;

    private void Update()
    {
        if (coolTimeFlag)
            time += Time.deltaTime;

        if (time > coolTime)
        {
            time = 0;
            coolTimeFlag = false;
        }
    }

    // UIManagerÇ≈égÇ§Getä÷êî
    public bool IsCooling()
    {
        return coolTimeFlag;
    }

    

    public float GetCooldownProgress()
    {
        return time / coolTime;
    }

    public void Activation()
    {
        if (!coolTimeFlag)
        {
            magicObject.transform.localScale = scale;
            Instantiate(magicObject, transform.position + adjustPos, Quaternion.identity);
            StartCoolTime();
        }
    }

    public void StartCoolTime()
    {
        coolTimeFlag = true;
    }

    public float GetAdjustPos_x()
    {
        return adjustPos.x;
    }

    public float GetCoolTime()
    {
        return coolTime;
    }

    public void SetAdjustPos_x(float x)
    {
        adjustPos.x = x;
    }

    public void SetCoolTime(float f)
    {
        if (f < 0) return;

        coolTime = f;
    }

    public void SetMagic(int direction)
    {
        scale = new Vector3(Mathf.Abs(scale.x) * direction, scale.y, scale.z);
    }
}
