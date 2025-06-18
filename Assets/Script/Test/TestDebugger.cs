using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestDebugger : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
        Debug.Log("enum0 : " + (int)TestEnum.zero);
        Debug.Log("enum1 : " + (int)TestEnum.one);
        Debug.Log("enum2 : " + (int)TestEnum.two);
        Debug.Log("enum3 : " + (int)TestEnum.three);
        Debug.Log("enum4 : " + (int)TestEnum.four);
        Debug.Log("enum5 : " + (int)TestEnum.five);

        Debug.Log("flag0 : " + Convert.ToString((int)TestFlag.zero, 2));
        Debug.Log("flag1 : " + Convert.ToString((int)TestFlag.one, 2));
        Debug.Log("flag2 : " + Convert.ToString((int)TestFlag.two, 2));
        Debug.Log("flag3 : " + Convert.ToString((int)TestFlag.three, 2));
        Debug.Log("flag4 : " + Convert.ToString((int)TestFlag.four, 2));
        Debug.Log("flag5 : " + Convert.ToString((int)TestFlag.five, 2));
    }
}
