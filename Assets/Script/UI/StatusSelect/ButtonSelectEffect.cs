using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectEffect : MonoBehaviour,ISelectHandler
{
    //ボタンが選択状態になったら実行される
    public void OnSelect(BaseEventData eventData)
    {
        //SEを流す
        AudioManager.Instance.PlaySE(AudioType.cursor);
        SelectAction();
    }

    //クリックされたときのその他エフェクトをオーバーライドで実装
    protected virtual void SelectAction()
    {
        //Debug.Log("Select Button : " + name);
    }
}
