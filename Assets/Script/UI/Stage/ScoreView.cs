using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    public void ManualStart()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    //���_�e�L�X�g�̕\���X�V
    public void UpdateText(ScoreModel model)
    {
        textMesh.text = model.Score.ToString();
    }
}
