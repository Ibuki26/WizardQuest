using UnityEngine;
using TMPro;

public class ModeButtonEffect : ButtonSelectEffect
{
    [SerializeField, TextArea(2, 3)] private string introduceText;
    [SerializeField] private TextMeshProUGUI textMesh;

    protected override void SelectAction()
    {
        base.SelectAction();
        textMesh.text = introduceText;
    }
}
