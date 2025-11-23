using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class PlayDataRecorder : SingletonMonoBehaviour<PlayDataRecorder>
{
    private int jumpCount = 0; //ジャンプ回数
    private int[] castCounts = new int[2]; //魔法使用回数
    private int totalDamage = 0; //プレイヤーの被弾ダメージ数
    private int totalAttack = 0; //敵キャラに与えたダメージの合計値
    private int damageCount = 0; //ダメージ被弾回数
    private int getHeart = 0; //回復アイテムの取得回数
    private int getCoin = 0; //コインの取得回数
    private float moveLength = 0f; //合計移動距離
    private float playTime = 0f; //プレイ時間
    //オーキャン2日目から追加したデータ
    private int turnCount = 0; //方向転換した回数
    private int score = 0; //スコア
    private int enemyDethCount = 0; //敵を倒した回数
    private int magicHitCount = 0; //魔法が敵に当たった回数

    public string playerID;
    private int oldDirection = 1;

    private void Start()
    {
        jumpCount = 0;
        castCounts[0] = 0;
        castCounts[1] = 0;
        totalDamage = 0;
        totalAttack = 0;
        damageCount = 0;
        getHeart = 0;
        getCoin = 0;
        moveLength = 0;
        playTime = 0;
        turnCount = 0;
        score = 0;
        oldDirection = 1;
        enemyDethCount = 0;
        magicHitCount = 0;
        GenerateID();
    }

    public void GenerateID()
    {
        System.Random random = new System.Random();

        // アルファベット1文字（A～Z）
        char letter = (char)('A' + random.Next(0, 26));

        // 数字2桁（00～99）
        int number = random.Next(0, 100);

        playerID = letter + number.ToString();
    }

    public void AddJump()
    {
        jumpCount++;
    }

    public void AddCastCount(int n)
    {
        castCounts[n]++;
    }

    public void AddTotalDamage(int damage)
    {
        totalDamage += damage;
    }

    public void AddTotalAttack(int attack)
    {
        totalAttack += attack;
    }

    public void AddDamageCount()
    {
        damageCount++;
    }

    public void AddGetHaert()
    {
        getHeart++;
    }

    public void AddGetCoin()
    {
        getCoin++;
    }

    public void AddMoveLength(float length)
    {
        moveLength += length;
    }

    public void AddPlayTime(float time)
    {
        playTime += time;
    }

    public void CheckTurn(int direction)
    {
        if (direction == oldDirection) return;

        turnCount++;
        oldDirection = direction;
    }

    public void SetScore(int value)
    {
        score = value;
    }

    public void AddEnemyDethCount()
    {
        enemyDethCount++;
    }

    public void AddMagicHitCount() => magicHitCount++;

    public void SaveDataToCSV_Multi()
    {
        string folderPath = Application.persistentDataPath + "/PlayData"; // 保存フォルダ
        Debug.Log(folderPath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = folderPath + "/playdata_all.csv";

        bool fileExists = File.Exists(filePath);
        using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
        {
            // ファイルがない場合はヘッダーを先に書く
            if (!fileExists)
            {
                sw.WriteLine("PlayerID,JumpCount,CastCount1,CastCount2,TotalDamage,TotalAttack,DamageCount,GetHeart,GetCoin,MoveLength,PlayTime,Magic1,Magic2,Equipment1,Equipment2,TurnCount,Score,EnemyDethCount,MagicHitCount");
            }

            var magics = MyStatusManager.Instance.FetchMagic();
            var equipments = MyStatusManager.Instance.FetchEquipment();
            
            //一回目のオーキャンから魔法、武器の記録名の変更
            //振り向き回数、スコアの記録を追加した

            // データを1行として追記
            sw.WriteLine($"{playerID},{jumpCount},{castCounts[0]},{castCounts[1]},{totalDamage},{totalAttack},{damageCount},{getHeart},{getCoin},{moveLength},{playTime},{magics[0].magicName},{magics[1].magicName},{equipments[0].equipmentName},{equipments[1].equipmentName},{turnCount},{score},{enemyDethCount},{magicHitCount}");
        }
    }
}
