using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ApplyShowedUI : MonoBehaviour
{
    //�v���C���[�p��UI�\��
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image[] magicIcons = new Image[2];
    [SerializeField] private SetButtonScroller scroller;

    //�ύX���s���I�u�W�F�N�g�̓o�^
    [SerializeField] private GameObject[] applyedObjects;

    void Start()
    {
        foreach(var obj in applyedObjects)
        {
            if(obj.TryGetComponent<SetMagicButton>(out var setMagic))
            {
                setMagic.SetMagicIcons(magicIcons);
                setMagic.SetTextMeshProUGUI(textMesh);
            }

            if(obj.TryGetComponent<MagicButtonEffect>(out var effecter))
            {
                effecter.SetScroller(scroller);
                effecter.SetTextMeshProUGUI(textMesh);
            }
        }
    }
}
