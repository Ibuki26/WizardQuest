using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    private Image image;

    public void ManualStart()
    {
        image = GetComponent<Image>();
        image.DOFillAmount(0f, 1.0f);
    }
}
