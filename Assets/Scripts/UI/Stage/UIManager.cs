using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using TMPro;
using Cysharp.Threading.Tasks.Triggers;

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
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Canvas canvas;

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

    //ダメージの数値を画面に表示する関数
    public void CreateDamageText(Vector3 pos, int damage, float height)
    {
        var createdPos = pos + Vector3.up * height;

        var createdText = Instantiate(damageText.gameObject, createdPos, Quaternion.identity,  canvas.transform).GetComponent<TextMeshProUGUI>();

        createdText.SetText(damage.ToString());

        Destroy(createdText.gameObject, 0.5f);
    }
}
