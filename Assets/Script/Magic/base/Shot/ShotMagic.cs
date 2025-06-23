using UnityEngine;

public abstract class ShotMagic : MonoBehaviour
{
    protected ShotMagicCreatorStatus _status; //魔法の情報
    protected WizardModel _model; //プレイヤーの情報

    public ShotMagicCreatorStatus Status => _status;
    public WizardModel Model => _model;

    //変数への情報の登録
    public void Initialize(ShotMagicCreatorStatus status, WizardModel model)
    {
        _status = status;
        _model = model;
    }

    //生成後の魔法の挙動
    public abstract void Action();
}
