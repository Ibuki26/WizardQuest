
public class EnemyStateController
{
    private EnemyState currentState; //プレイヤーの現在の状況を記録する

    //インスタンスの生成
    public EnemyStateController()
    {
        currentState = default;
    }

    //WizardStateをセットする関数。ManualStartで呼ぶ
    public void SetState(EnemyState set)
    {
        currentState = set;
    }

    //WizardStateを追加する関数
    public void AddState(EnemyState add)
    {
        currentState |= add;
    }

    //WizardStateを削除する関数
    public void DeleteState(EnemyState delete)
    {
        currentState &= ~delete;
    }

    //指定したWizardStateの状態を持つか確認する関数
    public bool HasState(EnemyState check)
    {
        return currentState.HasFlag(check);
    }
}
