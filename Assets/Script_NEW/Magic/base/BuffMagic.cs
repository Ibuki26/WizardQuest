using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using WizardPlayer;

namespace WizardMagic
{
    public abstract class BuffMagic : Magic,Magic.CreateMethod
    {
        private BuffMagic createdMagic;

        public void Create(MagicCreatorStatus status, WizardPresenter player, float destroyTime)
        {
            createdMagic = Instantiate(gameObject, player.transform.position + status.AdjustCreatePoint, Quaternion.identity).GetComponent<BuffMagic>();
            //Wizardの子オブジェクトにし、エフェクトをWizard中心に発生するようにする
            createdMagic.transform.parent = player.transform;
            //魔法の向きの設定
            var magicScale = transform.localScale;
            createdMagic.transform.localScale = new Vector3(magicScale.x * player.Model.Direction, magicScale.y, magicScale.z);
            createdMagic.Buff(player, destroyTime);
        }

        //Buffの効果
        public abstract UniTask Buff(WizardPresenter player, float destroyTime);

        //効果解除時の処理
        public abstract void Deactivate(WizardPresenter player);
    }
}
