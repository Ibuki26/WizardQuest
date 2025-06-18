using UnityEngine;
using TMPro;
using DG.Tweening;

public class BlinkText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private bool blinking = false;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!blinking)
        {
            blinking = true;
            textMesh.DOFade(0f, 2.0f)
                .OnComplete(() =>
                textMesh.DOFade(1f, 2.0f)
                .OnComplete(() => blinking = false));
        }
    }
}
