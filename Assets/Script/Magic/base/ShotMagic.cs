using UnityEngine;

public abstract class ShotMagic : Magic, Magic.CreateMethod
{
    private int attack;
    private int strength;
    private ShotMagic createdMagic;

    public int Attack => attack; //魔法の威力を記録する変数
    public int Strength => strength; //Playerの攻撃力を記録する変数

    //魔法の生成と向き、威力の設定
    //引数は魔法のステータス、生成場所(プレイヤーのいる座標)、プレイヤーの向き
    public void Create(ShotMagicCreatorStatus status, Vector3 playerPosition, int direction)
    {
        //生成位置の計算
        var adjustCreatePoint = new Vector3(status.AdjustCreatePoint.x * direction, status.AdjustCreatePoint.y, status.AdjustCreatePoint.z);
        createdMagic = Instantiate(gameObject, playerPosition + adjustCreatePoint, Quaternion.identity).GetComponent<ShotMagic>();
        //変数のデータ更新
        createdMagic.attack = status.Attack;
        createdMagic.strength = status.WizardStrength;
        //魔法の向きの設定
        var magicScale = transform.localScale;
        createdMagic.transform.localScale = new Vector3(magicScale.x * direction, magicScale.y, magicScale.z);
        createdMagic.Action(status.Speed * direction, status.DestroyTime);
    }

    //生成後の魔法の挙動
    protected abstract void Action(float speed, float destroyTime);

    //Enemyに当たったときの処理
    public abstract void Effect(EnemyPresenter enemy);
}
