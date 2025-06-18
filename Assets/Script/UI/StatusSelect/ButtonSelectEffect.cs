using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectEffect : MonoBehaviour,ISelectHandler
{
    //�{�^�����I����ԂɂȂ�������s�����
    public void OnSelect(BaseEventData eventData)
    {
        //SE�𗬂�
        AudioManager.Instance.PlaySE(AudioType.cursor);
        SelectAction();
    }

    //�N���b�N���ꂽ�Ƃ��̂��̑��G�t�F�N�g���I�[�o�[���C�h�Ŏ���
    protected virtual void SelectAction()
    {
        //Debug.Log("Select Button : " + name);
    }
}
