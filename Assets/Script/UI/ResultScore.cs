using UnityEngine;
using TMPro;

public class ResultScore : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] private ScorePresenter score;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "スコア：" + score.Model.Score;
    }
}
