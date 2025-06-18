using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class HitPointBar : MonoBehaviour
{
    private WizardPresenter player;
    private Image image;
    private int oldHp; //�O��Hitpoint

    public void ManualStart(WizardPresenter player)
    {
        this.player = player;
        image = GetComponent<Image>();
        oldHp = player.Model.HitPoint.Value;
        Bind();
    }

    //�v���C���[�̗̑͂��ω������Ƃ��̏�����o�^
    private void Bind()
    {
        player.Model.HitPoint
            .Subscribe(value =>
            {
                    //HitPoint������������
                    if (value < oldHp)
                    DecreaseBar(value);
                    //HitPoint������������
                    else if (value > oldHp)
                    IncreaseBar(value);

                oldHp = value;
            })
            .AddTo(gameObject);
    }

    private void IncreaseBar(int currentHp)
    {
        //������Hitpoint���������ABar�̈ړ����������悤�Ɍv�Z
        image.DOFillAmount((float)currentHp / player.Model.MaxHitPoint, (float)oldHp / currentHp * 10);
    }

    private void DecreaseBar(int currentHp)
    {
        //������Hitpoint���������ABar�̈ړ����������悤�Ɍv�Z
        image.DOFillAmount((float)currentHp / player.Model.MaxHitPoint, (float)currentHp / oldHp);
    }
}
