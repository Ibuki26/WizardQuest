using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//魔法、装備をセットするボタンの選択時のスクロール操作を行うクラス
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

        //右にスクロール
        if(newPosX > selectedPosX)
        {
            //DoHorizontalNormalPos　第1引数　移動先のx座標、第2引数　移動時間
            scrollRect.DOHorizontalNormalizedPos(scrollValue + 0.2f, 0.2f);
        }
        //左にスクロール
        else if(newPosX < selectedPosX)
        {
            scrollRect.DOHorizontalNormalizedPos(scrollValue - 0.2f, 0.2f);
        }

        selectedPosX = newPosX;
    }
}
