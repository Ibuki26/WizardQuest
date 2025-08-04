using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentName;
    [SerializeField, TextArea(2, 3)] public string introduceText;
    public Sprite icon;
    public SkillBase skill;

    public int hitPoint;
    public int strength;
    public int defense;
    public int speed;
}
