using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Tilemaps;

public class ConditionManager : MonoBehaviour
{
    private List<Condition> conditions = new List<Condition>();
    private List<TriggerCondition> triggerConditions = new List<TriggerCondition>();
    private WizardPresenter wizard;
    private EnemyPresenter enemy;

    public void ManualStart()
    {
        wizard = GetComponent<WizardPresenter>();
        enemy = GetComponent<EnemyPresenter>();
    }

    //Conditionのリストの追加と効果の実行を行う関数
    public async UniTask AddAndRun(Condition condition)
    {
        //リストに同じConditionがあるか確認、あったら終了
        if (CheckCondition(condition)) return;

        var token = this.GetCancellationTokenOnDestroy();
        //リストに追加
        conditions.Add(condition);

        //効果の実行
        if(wizard != null)
        {
            await condition.Effect(wizard, token);
        }
        else if(enemy != null)
        {
            await condition.Effect(enemy, token);
        }

        //リストから削除
        conditions.Remove(condition);
    }

    //TriggerConditionのリストの追加と効果の実行を行う関数
    public void AddAndRun(TriggerCondition triggerCondition)
    {
        if (CheckTriggerCondition(triggerCondition)) return;

        //条件が達成されているか確認
        bool triggered = false;
        if(wizard != null)
        {
            triggered = triggerCondition.Effect(wizard);
        }
        else if(enemy != null)
        {
            triggered = triggerCondition.Effect(enemy);
        }
        
        //条件が達成されていない場合、効果が起動しないため追加しない
        if(triggered)
            triggerConditions.Add(triggerCondition);
    }

    //TriggerConditionのリストから削除を行う関数
    public void Remove(TriggerCondition triggerCondition)
    {
        if (!CheckTriggerCondition(triggerCondition)) return;

        TriggerCondition removeCondition = null;
        for(int i = 0; i < triggerConditions.Count; i++)
        {
            if (triggerCondition.conditionName == triggerConditions[i].conditionName)
                removeCondition = triggerConditions[i];
        }

        //条件が達成されているか確認
        bool triggered = false;
        if (wizard != null)
        {
            triggered = triggerCondition.RemoveEffect(wizard);
        }
        else if (enemy != null)
        {
            triggered = triggerCondition.RemoveEffect(enemy);
        }

        //条件が達成されていない場合、解除されない
        if (triggered)
            triggerConditions.Remove(removeCondition);
    }

    //リストに引数のConditionがあるか確認する関数
    private bool CheckCondition(Condition condition)
    {
        foreach(var con in conditions)
        {
            if (con.conditionName == condition.conditionName)
                return true;
        }

        return false;
    }

    //リストに引数のTriggerConditionがあるか確認する関数
    private bool CheckTriggerCondition(TriggerCondition triggerCondition)
    {
        foreach(var con in triggerConditions)
        {
            if (con.conditionName == triggerCondition.conditionName)
                return true;
        }

        return false;
    }
}
