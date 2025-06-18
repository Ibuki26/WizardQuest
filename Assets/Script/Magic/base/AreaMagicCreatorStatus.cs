using UnityEngine;

public class AreaMagicCreatorStatus : MagicCreatorStatus
{
    [SerializeField] private int _attack; //���@�̈З�
    private int _wizardStrength; //�v���C���[��Strength���L�^����ϐ�

    #region getter,setter
    public int Attack
    {
        get { return _attack; }
        set
        {
            if (value < 0)
            {
                Debug.Log("_attack�ւ̑�������̒l�ł��B");
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
