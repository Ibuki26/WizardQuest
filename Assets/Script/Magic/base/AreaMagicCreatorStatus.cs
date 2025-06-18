using UnityEngine;

public class AreaMagicCreatorStatus : MagicCreatorStatus
{
    [SerializeField] private int _attack; //魔法の威力
    private int _wizardStrength; //プレイヤーのStrengthを記録する変数

    #region getter,setter
    public int Attack
    {
        get { return _attack; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_attackへの代入が負の値です。");
                return;
            }
            _attack = value;
        }
    }

    public int WizardStrength
    {
        get { return _wizardStrength; }
        set { _wizardStrength = value; }
    }
    #endregion
}
