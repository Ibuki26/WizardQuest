using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScoreText : MonoBehaviour
{
    [SerializeField] private UIManager ui;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "スコア：" + ui.GetScore();
    }
}
