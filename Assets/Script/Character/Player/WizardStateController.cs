using UnityEngine;

public class WizardStateController : MonoBehaviour
{
    private WizardState currentState; //プレイヤーの現在の状況を記録する

    //WizardStateをセットする関数。ManualStartで呼ぶ
    public void SetState(WizardState set)
    {
        currentState = set;
    }

    //WizardStateを追加する関数
    public void AddState(WizardState add)
    {
        currentState |= add;
    }

    //WizardStateを削除する関数
    public void DeleteState(WizardState delete)
    {
        currentState &= ~delete;
    }

    //指定したWizardStateの状態を持つか確認する関数
    public bool HasState(WizardState check)
    {
        return currentState.HasFlag(check);
    }
}