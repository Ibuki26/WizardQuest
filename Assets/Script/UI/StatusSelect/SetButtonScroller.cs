using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//���@�A�������Z�b�g����{�^���̑I�����̃X�N���[��������s���N���X
public class SetButtonScroller : MonoBehaviour
{
    private float selectedPosX = 0;
    private ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void ScrollSetButtonBar(float newPosX)
    {
        if (selectedPosX == 0) selectedPosX = newPosX;
        var scrollValue = scrollRect.horizontalScrollbar.value;

        //�E�ɃX�N���[��
        if(newPosX > selectedPosX)
        {
            //DoHorizontalNormalPos�@��1�����@�ړ����x���W�A��2�����@�ړ�����
            scrollRect.DOHorizontalNormalizedPos(scrollValue + 0.2f, 0.2f);
        }
        //���ɃX�N���[��
        else if(newPosX < selectedPosX)
        {
            scrollRect.DOHorizontalNormalizedPos(scrollValue - 0.2f, 0.2f);
        }

        selectedPosX = newPosX;
    }
}
