
//魔法が当たったときに発動するスキル用インターフェイス
namespace Skill
{
    public interface IOnMagicHit
    {
        void OnMagicHit(EnemyModel enemyModel);
    }
}