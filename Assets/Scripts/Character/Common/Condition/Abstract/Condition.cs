using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

//毒やステータス弱体化といった状態異常やステータス強化といった状態変化の抽象クラス
public abstract class Condition
{
    public Condition(string name, float time, Sprite sprite)
    {
        conditionName = name;
        duration = time;
        icon = sprite;
    }

    public string conditionName; //状態変化の名前

    public float duration; //効果時間

    public Sprite icon; //効果発動中のアイコン

    //プレイヤーへの効果
    public abstract UniTask Effect(WizardPresenter wizard, CancellationToken token);

    //敵キャラクターへの効果
    public abstract UniTask Effect(EnemyPresenter enemy, CancellationToken token);
}
