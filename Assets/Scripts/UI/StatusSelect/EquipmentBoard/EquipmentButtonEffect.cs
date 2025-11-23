using TMPro;

public class EquipmentButtonEffect : ButtonSelectEffect
{
    private TextMeshProUGUI textMesh; //魔法の紹介文を表示するオブジェクト
    private SetEquipmentButton equipmentButton;
    private ShowedStatusManager manager;
    private SetButtonScroller scroller;

    #region setter
    public void SetScroller(SetButtonScroller scroller) => this.scroller = scroller;

    public void SetManager(ShowedStatusManager manager) => this.manager = manager;

    public void SetTextMeshProUGUI(TextMeshProUGUI textMeshProUGUI) =>   textMesh = textMeshProUGUI;
    #endregion

    private void Start()
    {
        equipmentButton = GetComponent<SetEquipmentButton>();
    }

    protected override void SelectAction()
    {
        base.SelectAction();
        //文章の変更
        var equip = equipmentButton.Equipment;
        textMesh.text = equip.equipmentName + "\n体力：" + equip.hitPoint + "、攻撃：" 
            + equip.strength + "\n防御：" + equip.defense + "、速さ：" + equip.speed
            + "\n" + equip.introduceText;
        //ステータスの更新
        manager.ChangeAfterStatus(equipmentButton.Equipment, equipmentButton.Num);
        //スクロールビューのスクロール
        scroller.ScrollSetButtonBar(transform.position.x);
    }
}
