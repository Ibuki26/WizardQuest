using UnityEngine;

public class MyStatusManager : SingletonMonoBehaviour<MyStatusManager>
{
    [SerializeField] private MyStatus myStatus;

    public void UpdateMagic(MagicCreatorStatusData data, int num)
    {
        myStatus.magics[num] = data;
    }

    public void UpdateEquipment(Equipment equipment, int num)
    {
        myStatus.equipments[num] = equipment;
    }

    public MagicCreatorStatusData[] FetchMagic() => myStatus.magics;

    public Equipment[] FetchEquipment() => myStatus.equipments;
}
