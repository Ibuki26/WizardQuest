using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePrehab : MonoBehaviour
{
    [SerializeField] private PrefabValue pre;

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Value");
            pre.ChangeValue(5);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Tuen");
            pre.Turn();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("String");
            pre.ChangeString("sss");
        }
    }
}
