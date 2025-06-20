using UnityEngine;
using System.Linq;
using Skill;

//スキルの登録と実行を管理するクラス
public class SkillManager : MonoBehaviour
{
    private SkillBase[] skills = new SkillBase[2]; //スキルを記録する配列

    //スキルの登録
    public void RegisterSkills(SkillBase[] skills, WizardModel model) 
    {
        this.skills = skills;
        foreach (var skill in skills)
            skill.Initialize(model);
    }

    //ゲーム開始時のスキルの実行
    public void TriggerOnGameStart(MagicCreator[] magics)
    {
        foreach (var skill in skills.OfType<IOnGameStart>())
            skill.OnGameStart(magics);
    }

    //魔法発動時のスキルの実行
    public void TriggerOnMagicCast()
    {
        foreach (var skill in skills.OfType<IOnMagicCast>())
            skill.OnMagicCast();
    }

    //魔法が当たった時のスキルの実行
    public void TriggerOnMagicHit(EnemyModel enemyModel)
    {
        foreach (var skill in skills.OfType<IOnMagicHit>())
            skill.OnMagicHit(enemyModel);
    }
}
