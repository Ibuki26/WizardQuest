using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSetEquipment : MonoBehaviour
{
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private int num;

    // Start is called before the first frame update
    void Start()
    {
        equipmentIcon.sprite = MyStatusManager.Instance.FetchEquipment()[num].icon;
    }
}
