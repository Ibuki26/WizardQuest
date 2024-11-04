using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStatus : MonoBehaviour
{
    //装備ボタンの選択、ボタンの画像の変更
    [SerializeField] private Image[] buttons = new Image[3];
    private SelectedLamp lamp;
    private int seletNum = 0;

    private void Start()
    {
        lamp = GetComponent<SelectedLamp>();
    }

    public void SetNumber(int i)
    {
        if (i < 0) return;

        lamp.ChangeImage(seletNum, i);
        seletNum = i;
    }

    public void AccessMagic(GameObject m)
    {
        if (seletNum == 2) return;

        MySetedMagic.Instance.SetMagic(m, seletNum);
    }

    public void CreateWand(Wand wand)
    {
        if (seletNum != 2) return;

        MySetedMagic.Instance.SetWand(wand);
    }

    public void ChangeGraphic(Sprite sprite, int kindNumber)
    {
        if (kindNumber == 0 && seletNum == 2) return;
        if (kindNumber == 1 && seletNum != 2) return;

        buttons[seletNum].sprite = sprite;
    }
}
