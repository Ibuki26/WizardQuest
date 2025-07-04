using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentName;
    public Sprite icon;
    public SkillBase skill;

    public int hp;
    public int strength;
    public int defense;
    public int speed;
}
