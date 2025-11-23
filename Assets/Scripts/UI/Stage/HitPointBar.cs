using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class HitPointBar : MonoBehaviour
{
    private WizardModel model;
    private Image image;
    private int oldHp; //前のHitpoint

    public void ManualStart(WizardModel model)
    {
        this.model = model;
        image = GetComponent<Image>();
        oldHp = model.HitPoint.Value;
        Bind();
    }

    //プレイヤーの体力が変化したときの処理を登録
    private void Bind()
    {
        model.HitPoint
            .Subscribe(value =>
            {
                    //HitPointが減った処理
                    if (value < oldHp)
                        DecreaseBar(value);
                    //HitPointが増えた処理
                    else if (value > oldHp)
                        IncreaseBar(value);

                oldHp = value;
            })
            .AddTo(gameObject);
    }

    private void IncreaseBar(int currentHp)
    {
        //現在のアニメーションを停止
        image.DOKill();
        //増えたHitpointが多い程、Barの移動速が速いように計算
        image.DOFillAmount((float)currentHp / model.MaxHitPoint, (float)oldHp / currentHp);
    }

    private void DecreaseBar(int currentHp)
    {
        //現在のアニメーションを停止
        image.DOKill();
        //減ったHitpointが多い程、Barの移動速が速いように計算
        image.DOFillAmount((float)currentHp / model.MaxHitPoint, (float)currentHp / oldHp);
    }
}
