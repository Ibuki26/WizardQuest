using UnityEngine;
using TMPro;

public class ShowedStatusManager : MonoBehaviour
{
    // 0 : HitPont, 1 : Strength, 2 : Defense, 3 ; Speed
    //変更前のステータスを表示するText
    [SerializeField] private TextMeshProUGUI[] beforeText = new TextMeshProUGUI[4];
    //変更後のステータスを表示するText
    [SerializeField] private TextMeshProUGUI[] afterText = new TextMeshProUGUI[4];
    [SerializeField] private WizardModelData data;

    void Start()
    {
        SetBeforeStatus();
    }

    //最初のステータスのセット
    public void SetBeforeStatus()
    {
        var equipments = MyStatusManager.Instance.FetchEquipment();
        beforeText[0].text = (data.hitPoint + equipments[0].hitPoint + equipments[1].hitPoint).ToString();
        beforeText[1].text = (data.strength + equipments[0].strength + equipments[1].strength).ToString();
        beforeText[2].text = (data.defense + equipments[0].defense + equipments[1].defense).ToString();
        beforeText[3].text = (data.speed + equipments[0].speed + equipments[1].speed).ToString();
    }

    //装備変更によるステータス変化の表示
    public void ChangeBeforeStatus(Equipment equipment, int num)
    {
        var anotherEquipment = MyStatusManager.Instance.FetchEquipment()[1 - num];
        beforeText[0].text = (data.hitPoint + equipment.hitPoint + anotherEquipment.hitPoint).ToString();
        beforeText[1].text = (data.strength + equipment.strength + anotherEquipment.strength).ToString();
        beforeText[2].text = (data.defense + equipment.defense + anotherEquipment.defense).ToString();
        beforeText[3].text = (data.speed + equipment.speed + anotherEquipment.speed).ToString();
    }

    //装備選択時のステータス変化の表示
    public void ChangeAfterStatus(Equipment equipment, int num)
    {
        var anotherEquipment = MyStatusManager.Instance.FetchEquipment()[1 - num];
        afterText[0].text = (data.hitPoint + equipment.hitPoint + anotherEquipment.hitPoint).ToString();
        afterText[1].text = (data.strength + equipment.strength + anotherEquipment.strength).ToString();
        afterText[2].text = (data.defense + equipment.defense + anotherEquipment.defense).ToString();
        afterText[3].text = (data.speed + equipment.speed + anotherEquipment.speed).ToString();
    }
}
