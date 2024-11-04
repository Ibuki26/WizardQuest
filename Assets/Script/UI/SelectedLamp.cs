using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedLamp : MonoBehaviour
{
    [SerializeField] private Image[] img = new Image[3];
    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;

    public void ChangeImage(int old, int latest) 
    {
        img[old].sprite = offImage;
        img[latest].sprite = onImage;
    }
}
