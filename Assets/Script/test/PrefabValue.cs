using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabValue : MonoBehaviour
{
    [SerializeField] private float a;
    [SerializeField] private string s;

    public void ChangeValue(float num)
    {
        a = num;
    }

    public void Turn()
    {
        a *= -1;
    }

    public void ChangeString(string s)
    {
        this.s = s;
    }
}
