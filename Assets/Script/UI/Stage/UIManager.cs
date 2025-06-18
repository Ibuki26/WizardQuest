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

    //スコアの加算
    public void AddScore(int addScore)
    {
        score.AddScore(addScore);
    }

    //Shot型のMagic用のクールタイム表示
    public void ShotDisplayCoolTime(MagicCreatorStatus status, int num)
    {
        coolTimeIcons[num].ShotDisplayCoolTime(status);
    }

    //Buff,Area型のMagic用の効果時間の表示
    public void BuffAndAreaDisplayEffectTime(MagicCreatorStatus status, int num)
    {
        coolTimeIcons[num].BuffAndAreaDisplayEffectTime(status);
    }

    //Buff,Area型のMagic用のクールタイム表示
    public void BuffAndAreaDisplayCoolTime(MagicCreatorStatus status, int num)
    {
        coolTimeIcons[num].BuffAndAreaDisplayCoolTime(status);
    }
}
