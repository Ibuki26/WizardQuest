using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualCanvasSize : MonoBehaviour
{
    void Start()
    {
        var canvas = transform.parent.GetComponent<RectTransform>();
        float width = canvas.rect.width;
        float height = canvas.rect.height;

        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, height);
    }
}
