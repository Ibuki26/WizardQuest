using UnityEngine;
using TMPro;

public class MagicButtonEffect : ButtonSelectEffect
{
    [SerializeField, TextArea(2, 3)] private string introduceText; //魔法の紹介文
    private TextMeshProUGUI textMesh; //魔法の紹介文を表示するオブジェクト
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
        //文章の変更
        textMesh.text = introduceText;
        //スクロールビューのスクロール
        scroller.ScrollSetButtonBar(transform.position.x);
    }
}
