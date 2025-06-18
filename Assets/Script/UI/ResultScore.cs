using UnityEngine;
using TMPro;

public class ResultScore : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] private ScorePresenter score;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "�X�R�A�F" + score.Model.Score;
    }
}
