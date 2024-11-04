using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlickText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float time = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if(time <= 2)
        {
            textMesh.color = new Color(0, 0, 0, 1 - time / 2);
        }
        else if( time > 2 && time <= 4)
        {
            textMesh.color = new Color(0, 0, 0, (time-2)/2);
        }
        else
        {
            time = 0;
        }
    }
}
