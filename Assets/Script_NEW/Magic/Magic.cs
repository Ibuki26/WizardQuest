using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public abstract class Magic : MonoBehaviour
    {
        private int attack;
        public int Attack => attack;

        //魔法の生成と向き、威力の設定
        public void Create(MagicCreatorStatus status)
        {
            Instantiate(gameObject, transform.position + status.AdjustPos, Quaternion.identity);
            attack = status.Attack;
            var magicScale = status.transform.localScale;
            transform.localScale = new Vector3(magicScale.x * status.Direction, magicScale.y, magicScale.z);
            Action(status.Speed * status.Direction, status.DisappearTime);
        }

        //生成後の魔法の挙動
        protected abstract void Action(float speed, float disappearTime);

        //敵に当たったときの処理
        protected abstract void Effect(Enemy enemy);

        //プレイヤーに当たったときの処理
        protected abstract void Buff(Player player);
    }
}
