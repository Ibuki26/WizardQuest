using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicButton : MonoBehaviour
{
    [SerializeField] private GameObject magic;
    [SerializeField] private SetStatus status;
    private Button button;
    private Image img;
    private int kindNumber = 0;

    void Start()
    {
        button = GetComponent<Button>();
        img = GetComponent<Image>();
        button.onClick.AddListener(() => 
        { 
            AudioManager.Instance.PlaySE(AudioType.button);
            status.ChangeGraphic(img.sprite, kindNumber);
            status.AccessMagic(magic);
        });
    }
}
