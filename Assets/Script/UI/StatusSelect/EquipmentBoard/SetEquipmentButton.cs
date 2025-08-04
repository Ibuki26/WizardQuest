using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SetEquipmentButton : UIButtonBase
{
    [SerializeField] private Equipment equipment;
    [SerializeField] private int num;
    private Image equipmentImage;
    private ShowedStatusManager manager;

    public Equipment Equipment => equipment;
    public int Num => num;

    public void SetImage(Image image) => equipmentImage = image;

    public void SetManager(ShowedStatusManager manager) => this.manager = manager;

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => HandleSubmitAsync(PerformAction, num).Forget());
    }

    private void PerformAction(int num)
    {
        //SEを流す
        AudioManager.Instance.PlaySE(AudioType.button);
        //MyStatusへの装備の登録
        MyStatusManager.Instance.FetchEquipment()[num] = equipment;
        //画像の変更
        equipmentImage.sprite = equipment.icon;
        //ステータス表示の変更
        manager.ChangeBeforeStatus(equipment, num);
    }
}
