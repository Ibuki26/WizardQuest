using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WandButton : MonoBehaviour
{
    [SerializeField] private SetStatus status;
    [SerializeField] int num;
    private Button button;
    private Image img;
    private Wand[] wands = {new HitPointWand(30), new AttackWand(5), new QuickWand(0.2f) };
    private int kindNumber = 1;

    void Start()
    {
        button = GetComponent<Button>();
        img = GetComponent<Image>();
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(AudioType.button);
            status.ChangeGraphic(img.sprite, kindNumber);
            status.CreateWand(wands[num]);
        });
    }
}
