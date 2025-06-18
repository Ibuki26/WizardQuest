using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private WizardPresenter player;
    [SerializeField] private ScorePresenter score;
    [SerializeField] private CoolTImeIcon[] coolTimeIcons = new CoolTImeIcon[2];
    [SerializeField] private HitPointBar hitPointBar;

    public void ManualStart()
    {
        score.ManualStart();
        coolTimeIcons[0].ManualStart();
        coolTimeIcons[1].ManualStart();
        hitPointBar.ManualStart(player);
    }

    //�X�R�A�̉��Z
    public void AddScore(int addScore)
    {
        score.AddScore(addScore);
    }

    //Shot�^��Magic�p�̃N�[���^�C���\��
    public void ShotDisplayCoolTime(MagicCreatorStatus status, int num)
    {
        coolTimeIcons[num].ShotDisplayCoolTime(status);
    }

    //Buff,Area�^��Magic�p�̌��ʎ��Ԃ̕\��
    public void BuffAndAreaDisplayEffectTime(MagicCreatorStatus status, int num)
    {
        coolTimeIcons[num].BuffAndAreaDisplayEffectTime(status);
    }

    //Buff,Area�^��Magic�p�̃N�[���^�C���\��
    public void BuffAndAreaDisplayCoolTime(MagicCreatorStatus status, int num)
    {
        coolTimeIcons[num].BuffAndAreaDisplayCoolTime(status);
    }
}
