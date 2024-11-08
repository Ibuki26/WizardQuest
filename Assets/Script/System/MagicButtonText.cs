using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicButtonText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI magicTextB;
    [SerializeField] private TextMeshProUGUI magicTextY;
    private Magic magicB;
    private Magic magicY;

    private void Update()
    {
        UpdateMagicText();
    }

    private void UpdateMagicText()
    {
        magicTextB.text = magicB.IsCooling() ? "まってて" : "いつでもOK";

        magicTextY.text = magicY.IsCooling() ? "まってて" : "いつでもOK";
    }

    public void SetMagic(Magic magic1, Magic magic2)
    {
        magicB = magic1;
        magicY = magic2;
    }
}