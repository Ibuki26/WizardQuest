using UnityEngine;

public abstract class TriggerCondition
{
    public TriggerCondition(string name, Sprite sprite)
    {
        conditionName = name;
        icon = sprite;
    }

    public string conditionName; //状態変化の名前

    public Sprite icon; //効果発動中のアイコン

    //プレイヤーへの効果
    public abstract bool Effect(WizardPresenter wizard);

    //敵キャラクターへの効果
    public abstract bool Effect(EnemyPresenter enemy);

    //プレイヤーへの効果の解除
    public abstract bool RemoveEffect(WizardPresenter wizard);

    //敵キャラクターへの効果の解除
    public abstract bool RemoveEffect(EnemyPresenter enemy);
}
