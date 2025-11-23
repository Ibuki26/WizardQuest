using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApplyEquipmentUI : MonoBehaviour
{
    //プレイヤー用のUI表示
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;
    [SerializeField] private ShowedStatusManager manager;
    [SerializeField] private SetButtonScroller scroller;

    //変更を行うオブジェクトの登録
    [SerializeField] private GameObject[] applyedObjects;

    void Start()
    {
        foreach (var obj in applyedObjects)
        {
            if(obj.TryGetComponent<SetEquipmentButton>(out var button))
            {
                button.SetImage(image);
                button.SetManager(manager);
            }

            if(obj.TryGetComponent<EquipmentButtonEffect>(out var effect))
            {
                effect.SetManager(manager);
                effect.SetScroller(scroller);
                effect.SetTextMeshProUGUI(textMesh);
            }
        }
    }
}
