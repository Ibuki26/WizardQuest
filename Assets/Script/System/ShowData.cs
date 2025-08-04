using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowData : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    public void Show()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        var id = PlayDataRecorder.Instance.playerID;
        var magics = MyStatusManager.Instance.FetchMagic();
        var equipments = MyStatusManager.Instance.FetchEquipment();
        textMesh.text = "ID : " + id + "\n–‚–@‚PF" + magics[0].magicName
            + "\n–‚–@‚QF" + magics[1].magicName + "\n‘•”õ‚PF" + equipments[0].equipmentName
            + "\n‘•”õ‚QF" + equipments[1].equipmentName;
    }
}
