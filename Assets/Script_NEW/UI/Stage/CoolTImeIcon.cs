using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using WizardMagic;

public class CoolTImeIcon : MonoBehaviour
{
    private Image image;

    public void ManualStart()
    {
        image = GetComponent<Image>();
    }

    //Shot�^��Magic�p�̃N�[���^�C���\��
    public void ShotDisplayCoolTime(MagicCreatorStatus status)
    {
        //�摜�̕\���͈͂�0�ɂ���
        image.fillAmount = 0f;
        //CoolTime�̎��Ԃ������āA�摜�̕\���͈͂�S���ɂ���
        image.DOFillAmount(1.0f, status.CoolTime);
    }

    //Buff,Area�^��Magic�p�̃N�[���^�C���\��
    public void BuffAndAreaDisplayEffectTime(MagicCreatorStatus status)
    {
        //Buff,Area�̌��ʎ��Ԃ������āA�摜�̕\���͈͂�0�ɂ���
        image.DOFillAmount(0f, status.DestroyTime);
    }

    public void BuffAndAreaDisplayCoolTime(MagicCreatorStatus status)
    {
        //�N�[���^�C���̕\��
        image.DOFillAmount(1.0f, status.CoolTime);
    }
}
