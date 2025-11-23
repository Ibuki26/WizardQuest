using System.Linq;
using Skill;
using UnityEngine;

//スキルの登録と実行を管理するクラス
public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
    private SkillBase[] skills = new SkillBase[2]; //スキルを記録する配列

    //スキルの登録
    public void SetSkillBase(Equipment[] equipments)
    {
        //装備が無い場合は実行しない
        if (equipments[0] == null || equipments[1] == null) return;

        skills[0] = equipments[0].skill;
        skills[1] = equipments[1].skill;
    }

    //ゲーム開始時のスキルの実行
    public void TriggerOnGameStart(MagicCreator[] magics, ConditionManager conditionManager)
    {
        foreach (var skill in skills.OfType<IOnGameStart>())
            skill.OnGameStart(magics, conditionManager);
    }

    //魔法発動時のスキルの実行
    //現在はShot型用になっている
    public void TriggerOnMagicCast(GameObject magic)
    {
        foreach (var skill in skills.OfType<IOnMagicCast>())
            skill.OnMagicCast(magic);
    }

    //魔法が当たった時のスキルの実行
    //EnemyPresenterの被ダメージ時の処理で呼び出す
    public void TriggerOnMagicHit(MagicHitContext context)
    {
        foreach (var skill in skills.OfType<IOnMagicHit>())
            skill.OnMagicHit(context);
    }

    public void TriggerOnDamage(ConditionManager conditionManager)
    {
        foreach (var skill in skills.OfType<IOnDamage>())
            skill.OnDamage(conditionManager);
    }

    public void TriggerOnHeal(ConditionManager conditionManager)
    {
        foreach (var skill in skills.OfType<IOnHeal>())
            skill.OnHeal(conditionManager);
    }

    public void TriggerOnCollisionEnemy(EnemyPresenter enemy)
    {
        foreach (var skill in skills.OfType<IOnCollisionEnemy>())
            skill.OnCollisionEnemy(enemy);
    }
}
