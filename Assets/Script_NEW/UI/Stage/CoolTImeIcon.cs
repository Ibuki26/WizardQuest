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

    //Shot型のMagic用のクールタイム表示
    public void ShotDisplayCoolTime(MagicCreatorStatus status)
    {
        //画像の表示範囲を0にする
        image.fillAmount = 0f;
        //CoolTimeの時間をかけて、画像の表示範囲を全部にする
        image.DOFillAmount(1.0f, status.CoolTime);
    }

    //Buff,Area型のMagic用のクールタイム表示
    public void BuffAndAreaDisplayEffectTime(MagicCreatorStatus status)
    {
        //Buff,Areaの効果時間をかけて、画像の表示範囲を0にする
        image.DOFillAmount(0f, status.DestroyTime);
    }

    public void BuffAndAreaDisplayCoolTime(MagicCreatorStatus status)
    {
        //クールタイムの表示
        image.DOFillAmount(1.0f, status.CoolTime);
    }
}
