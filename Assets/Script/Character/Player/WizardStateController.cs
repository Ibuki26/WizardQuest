using UnityEngine;

public class WizardStateController : MonoBehaviour
{
    private WizardState currentState; //�v���C���[�̌��݂̏󋵂��L�^����

    //WizardState���Z�b�g����֐��BManualStart�ŌĂ�
    public void SetState(WizardState set)
    {
        currentState = set;
    }

    //WizardState��ǉ�����֐�
    public void AddState(WizardState add)
    {
        currentState |= add;
    }

    //WizardState���폜����֐�
    public void DeleteState(WizardState delete)
    {
        currentState &= ~delete;
    }

    //�w�肵��WizardState�̏�Ԃ������m�F����֐�
    public bool HasState(WizardState check)
    {
        return currentState.HasFlag(check);
    }
}