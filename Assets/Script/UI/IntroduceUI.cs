using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroduceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField, TextArea(2, 3)] private string introduceText;

    public void ChangeUI()
    {
        text.text = introduceText;
    }
}
