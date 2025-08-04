using UnityEngine;

//魔法を唱えたときに、発動するスキル用インターフェイス
namespace Skill
{
    public interface IOnMagicCast
    {
        //maigc：魔法のゲームオブジェクト
        void OnMagicCast(GameObject magic);
    }
}
