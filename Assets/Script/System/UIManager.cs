using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Player player; // Playerのインスタンス
    [SerializeField] private Magic[] magics; // Magicのインスタンス
    [SerializeField] private Image hitPointBar; // 体力バーのImageコンポーネント
    [SerializeField] private Image bButton; // BボタンのImageコンポーネント
    [SerializeField] private Image yButton; // YボタンのImageコンポーネント
    [SerializeField] private TextMeshProUGUI scoreText; // スコア表示用のTextコンポーネント
    [SerializeField] private GameObject clearWindow; //ゲームクリア時に映すオブジェクト
    [SerializeField] private GameObject overWindow; //ゲームオーバー時に映すオブジェクト
    [SerializeField] private GameObject optionWindow; //メニュー画面 
    private int score; // スコアの変数
    private float coolTime1 = 0;
    private float coolTime2 = 0;

    private void Start()
    {
        Cursor.visible = false;
        UpdateHitPointBar(player.GetHitPoint()); // 初期の体力バーの更新
        score = 0; // スコア初期化
        UpdateScoreDisplay(); // スコア表示の更新
    }

    
    private void Update()
    {
        if (magics[0].IsCooling())
            coolTime1 += Time.deltaTime;
        else
            coolTime1 = 0;

        if (magics[1].IsCooling())
            coolTime2 += Time.deltaTime;
        else
            coolTime2 = 0;

        UpdateHitPointBar(player.GetHitPoint()); // Get関数を使用して現在の体力を取得
        UpdateMagicCooldown(magics[0], bButton, coolTime1); // Bボタンのクールタイムを更新
        UpdateMagicCooldown(magics[1], yButton, coolTime2); // Yボタンのクールタイムを更新
    }

    // 体力バーの更新
    private void UpdateHitPointBar(int currentHitPoint)
    {
        float fillAmount = (float)currentHitPoint / player.GetMaxHitPoint(); // Get関数を使用
        hitPointBar.fillAmount = fillAmount;
    }

    // 魔法のクールタイムを更新
    private async void UpdateMagicCooldown(Magic magic, Image buttonImage, float coolTime)
    {
        if (magic.IsCooling()) // クールタイム中かを確認
        {
            buttonImage.fillAmount = 0; // FillAmountを0にする
            buttonImage.fillAmount = coolTime/magic.GetCoolTime();
            await UniTask.Delay((int)(magic.GetCoolTime() * 1000)); // クールタイムの待機
            buttonImage.fillAmount = 1; // FillAmountを1に戻す
        }
    }

    // スコアの加算処理
    public void AddScore(int value)
    {
        score += value; // スコア加算
        UpdateScoreDisplay(); // スコア表示を更新
    }

    // スコア表示を更新
    private void UpdateScoreDisplay()
    {
        scoreText.text = score.ToString(); // スコアを表示
    }

    public void SetMagic(Magic magic1, Magic magic2)
    {
        if (magic1 == null || magic2 == null) return;
        magics[0] = magic1;
        magics[1] = magic2;
    }

    public void ActiveWindow(string name)
    {
        if (name == "clear")
            clearWindow.SetActive(true);
        else if (name == "over")
            overWindow.SetActive(true);
        else if (name == "option")
            optionWindow.SetActive(true);
        else if (name == "mouse")
        {
            Cursor.visible = true;
        }
    }

    public void DisappearWindow(string name)
    {
        if (name == "option")
            optionWindow.SetActive(false);
        else if (name == "mouse")
        {
            Cursor.visible = false;
        }
    }

    public int GetScore()
    {
        return score;
    }
}
