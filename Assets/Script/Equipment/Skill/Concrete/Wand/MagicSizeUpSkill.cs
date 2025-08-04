using UnityEngine;
using Skill;


[CreateAssetMenu(menuName = "Skills/MagicSizeUp")]
public class MagicSizeUpSkill : SkillBase, IOnMagicCast
{
    [SerializeField] private float times; //ñÇñ@ÇÃScaleÇ…Ç©ÇØÇÈêîíl

    public void OnMagicCast(GameObject magic)
    {
        var magicSize = magic.transform.localScale;
        magic.transform.localScale = new Vector3(magicSize.x * times, magicSize.y * times, magicSize.z);
    }
}
