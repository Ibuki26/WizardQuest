using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MyStatus")]
public class MyStatus : ScriptableObject
{
    public  MagicCreatorStatusData[] magics = new MagicCreatorStatusData[2];
    public Equipment[] equipments = new Equipment[2];
}
