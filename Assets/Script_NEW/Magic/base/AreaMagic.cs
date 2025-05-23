using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardEnemy;

namespace WizardMagic
{
    public abstract class AreaMagic : Magic, Magic.CreateMethod
    {
        public GameObject Create(AreaMagicCreatorStatus status, Vector3 playerPosition, int direction)
        {
            return null;
        }

        //Enemyに当たったときの処理
        public abstract void Effect(EnemyPresenter enemy);
    }
}
