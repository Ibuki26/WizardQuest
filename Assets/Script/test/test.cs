using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class test : MonoBehaviour
{
    private TextMeshProUGUI textGUI;
    [SerializeField] private SightCheckerExample2D SEC;

    void Start()
    {
        textGUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (SEC.IsVisible())
        {
            textGUI.text = "Visible";
        }
        else
        {
            textGUI.text = "Invisible";
        }
    }
}
