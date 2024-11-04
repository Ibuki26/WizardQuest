using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class UniTaskTest : MonoBehaviour
{
    void Update()
    {
        Test();
    }

    private async void Test()
    {
        Debug.Log("start");
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        Debug.Log("finish");
    }
}
