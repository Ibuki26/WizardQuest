using UnityEngine;
using System.Linq;
using Skill;

//スキルの登録と実行を管理するクラス
public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
    [SerializeField] private SkillBase[] skills = new SkillBase[2]; //スキルを記録する配列

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
    //EnemyPresenterの被ダメージ時の処理で呼び出す
    public void TriggerOnMagicHit(MagicHitContext context)
    {
        foreach (var skill in skills.OfType<IOnMagicHit>())
            skill.OnMagicHit(context);
    }
}
