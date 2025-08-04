using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private WizardPresenter player;
    [SerializeField] private ScorePresenter score;
    [SerializeField] private CoolTImeIcon[] coolTimeIcons = new CoolTImeIcon[2];
    [SerializeField] private HitPointBar hitPointBar;
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private Button overButton;
    [SerializeField] private GameObject gameClearWindow;
    [SerializeField] private Button clearButton;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private Button optionButton;
    public void ManualStart()
    {
        score.ManualStart();
        coolTimeIcons[0].ManualStart();
        coolTimeIcons[1].ManualStart();
        hitPointBar.ManualStart(player.Model);
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

    //ゲームオーバー画面の表示
    public void ShowGameOver()
    {
        gameOverWindow.SetActive(true);
        eventSystem.SetSelectedGameObject(overButton.gameObject);
    }

    //ゲームクリア画面の表示
    public void ShowGameClear()
    {
        gameClearWindow.SetActive(true);
        eventSystem.SetSelectedGameObject(clearButton.gameObject);
    }

    //オプション画面の表示
    public void ShowOption()
    {
        optionWindow.SetActive(true);
        eventSystem.SetSelectedGameObject(optionButton.gameObject);
    }
}
