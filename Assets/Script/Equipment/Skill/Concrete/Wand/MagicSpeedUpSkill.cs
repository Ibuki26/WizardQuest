using UnityEngine;
using System.Linq;
using Skill;

[CreateAssetMenu(menuName = "Skills/MagicSpeedUp_Start")]
public class MagicSpeedUpSkill : SkillBase, IOnGameStart
{
    [SerializeField] private float UpValue; //速さに加算する数値

    public void OnGameStart(MagicCreator[] magics, WizardModel model)
    {
        foreach (var magic in magics.OfType<ShotMagicCreator>())
        {
            if (magic.Status.Speed == 0) return;

            //変更前のステータスの記録
            var beforeSpeed = magic.Status.Speed;
            var beforeDestroyTime = magic.Status.DestroyTime;
            //ステータスの変更
            magic.Status.Speed += UpValue;
            magic.Status.DestroyTime = beforeSpeed * beforeDestroyTime / magic.Status.Speed;
        }
    }
}
