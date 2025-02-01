using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using WizardPlayer;

namespace WizardMagic
{
    public class BuffMagicCreator : MagicCreator
    {
        private MagicCreatorStatus _status;

        public MagicCreatorStatus Status => _status;

        public BuffMagicCreator(MagicCreatorStatus status)
        {
            _status = status;
        }

        public async override void CreateMagic(WizardPresenter player, int num)
        {
            if (IsCoolTime) return;
            IsCoolTime = true;
            //魔法の生成とSE再生
            AudioManager.Instance.PlaySE(_status.ShotSound);
            BuffMagic buffMagic = (BuffMagic)_status.Magic;
            //クールタイムのUI表示
            WizardUI.UIManager.Instance.BuffAndAreaDisplayEffectTime(_status, num);
            buffMagic.Create(_status, player, _status.DestroyTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_status.DestroyTime));
            //効果の終了
            WizardUI.UIManager.Instance.BuffAndAreaDisplayCoolTime(_status, num);
            //クールタイム処理
            await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
            IsCoolTime = false;
        }
    }
}
