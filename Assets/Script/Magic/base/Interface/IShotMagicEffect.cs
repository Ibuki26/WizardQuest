
namespace ShotMagicMethod
{
    //魔法が敵に当たったときの効果
    public interface IShotMagicEffect
    {
        //引数にEnemyStateControllerも入れる
        void Effect(EnemyModel enemyModel);
    }
}
