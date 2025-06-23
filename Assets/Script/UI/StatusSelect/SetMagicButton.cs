using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class SetMagicButton : UIButtonBase, ICancelHandler
{
    [SerializeField] private MagicCreatorStatusData magic;
    private Image[] magicIcons = new Image[2]; //�Z�b�g���̖��@�̉摜��\������I�u�W�F�N�g
    private TextMeshProUGUI textMesh; //���@�̏Љ��\������I�u�W�F�N�g

    #region Setter
    public void SetMagicIcons(Image[] images) => magicIcons = images;

    public void SetTextMeshProUGUI(TextMeshProUGUI textMeshProUGUI) => textMesh = textMeshProUGUI;
    #endregion

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => HandleSubmitAsync(PerformAction, 0).Forget());
    }

    //Cancel���͂������Ƃ��̏���
    public void OnCancel(BaseEventData eventData)
    {
        HandleSubmitAsync(PerformAction, 1).Forget();
    }

    protected void PerformAction(int num)
    {
        //�Z�b�g����2�̖��@�������ɂȂ邩�̊m�F
        if (MyStatus.Instance.magics[1 - num] != magic)
        {
            //SE�𗬂�
            AudioManager.Instance.PlaySE(AudioType.button);
            //���삷�閂�@�̓o�^
            MyStatus.Instance.magics[num] = magic;
            //���@�A�C�R���̉摜�ύX
            magicIcons[num].sprite = magic.image;
        }
        //�������@���Z�b�g���悤�Ƃ����Ƃ����s����
        else
        {
            AudioManager.Instance.PlaySE(AudioType.buttonCancel);
            textMesh.text = "�������@��2�Z�b�g�ł��܂���B";
        }
    }
}
