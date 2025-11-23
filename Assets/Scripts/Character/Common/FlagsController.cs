using System;
using UnityEngine;

public class FlagsController<T> where T : Enum
{
    private T currentState; //現在の状況を記録する

    //インスタンスの生成
    public FlagsController()
    {
        currentState = default;

        // 任意：Flags属性が付いているかチェック（なくても使える）
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
        {
            Debug.LogWarning($"{typeof(T).Name} に [Flags] 属性が付いていません");
        }
    }

    //WizardStateをセットする関数。ManualStartで呼ぶ
    public void SetState(T set)
    {
        currentState = set;
    }

    //WizardStateを追加する関数
    public void AddState(T add)
    {
        currentState = (T)(object)((int)(object)currentState | (int)(object)add);
    }

    //WizardStateを削除する関数
    public void DeleteState(T delete)
    {
        currentState = (T)(object)((int)(object)currentState & ~(int)(object)delete);
    }

    //指定したWizardStateの状態を持つか確認する関数
    public bool HasState(T check)
    {
        return currentState.HasFlag(check);
    }
}
