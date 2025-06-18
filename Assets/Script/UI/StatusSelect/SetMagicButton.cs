using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class SetMagicButton : UIButtonBase, ICancelHandler
{
    [SerializeField] private MagicCreatorStatus magic;
    private Image[] magicIcons = new Image[2]; //セット中の魔法の画像を表示するオブジェクト
    private TextMeshProUGUI textMesh; //魔法の紹介文を表示するオブジェクト

    #region Setter
    public void SetMagicIcons(Image[] images) => magicIcons = images;

    public void SetTextMeshProUGUI(TextMeshProUGUI textMeshProUGUI) => textMesh = textMeshProUGUI;
    #endregion

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => HandleSubmitAsync(PerformAction, 0).Forget());
    }

    //Cancel入力がきたときの処理
    public void OnCancel(BaseEventData eventData)
    {
        HandleSubmitAsync(PerformAction, 1).Forget();
    }

    protected void PerformAction(int num)
    {
        //セットする2つの魔法が同じになるかの確認
        if (MyStatus.magics[1 - num] != magic)
        {
            //SEを流す
            AudioManager.Instance.PlaySE(AudioType.button);
            //操作する魔法の登録
            MyStatus.magics[num] = magic;
            //魔法アイコンの画像変更
            magicIcons[num].sprite = magic.Image;
        }
        //同じ魔法をセットしようとしたとき失敗する
        else
        {
            AudioManager.Instance.PlaySE(AudioType.buttonCancel);
            textMesh.text = "同じ魔法は2つセットできません。";
        }
    }
}
