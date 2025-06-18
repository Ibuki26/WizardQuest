using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class HitPointBar : MonoBehaviour
{
    private WizardPresenter player;
    private Image image;
    private int oldHp; //前のHitpoint

    public void ManualStart(WizardPresenter player)
    {
        this.player = player;
        image = GetComponent<Image>();
        oldHp = player.Model.HitPoint.Value;
        Bind();
    }

    //プレイヤーの体力が変化したときの処理を登録
    private void Bind()
    {
        player.Model.HitPoint
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
        //増えたHitpointが多い程、Barの移動速が速いように計算
        image.DOFillAmount((float)currentHp / player.Model.MaxHitPoint, (float)oldHp / currentHp * 10);
    }

    private void DecreaseBar(int currentHp)
    {
        //減ったHitpointが多い程、Barの移動速が速いように計算
        image.DOFillAmount((float)currentHp / player.Model.MaxHitPoint, (float)currentHp / oldHp);
    }
}
