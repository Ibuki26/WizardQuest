using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSetImage : MonoBehaviour
{
    [SerializeField] private Image[] magicIcon = new Image[2];

    // Start is called before the first frame update
    void Start()
    {
        var magics = MyStatusManager.Instance.FetchMagic();
        magicIcon[0].sprite = magics[0].image;
        magicIcon[1].sprite = magics[1].image;
    } 
}
