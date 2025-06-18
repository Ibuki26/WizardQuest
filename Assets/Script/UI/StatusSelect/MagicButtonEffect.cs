using UnityEngine;
using TMPro;

public class MagicButtonEffect : ButtonSelectEffect
{
    [SerializeField, TextArea(2, 3)] private string introduceText; //���@�̏Љ
    private TextMeshProUGUI textMesh; //���@�̏Љ��\������I�u�W�F�N�g
    private SetButtonScroller scroller;

    #region setter
    public void SetScroller(SetButtonScroller scroller)
    {
        this.scroller = scroller;
    }

    public void SetTextMeshProUGUI(TextMeshProUGUI textMeshProUGUI)
    {
        textMesh = textMeshProUGUI;
    }
    #endregion

    protected override void SelectAction()
    {
        base.SelectAction();
        //���͂̕ύX
        textMesh.text = introduceText;
        //�X�N���[���r���[�̃X�N���[��
        scroller.ScrollSetButtonBar(transform.position.x);
    }
}
