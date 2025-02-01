using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using WizardPlayer;

namespace WizardMagic
{
    public class ShotMagicCreator : MagicCreator
    {
        private ShotMagicCreatorStatus _status;

        public ShotMagicCreatorStatus Status => _status;

        public ShotMagicCreator(ShotMagicCreatorStatus status)
        {
            _status = status;
        }

        public async override void CreateMagic(WizardPresenter player, int num)
        {
            if (IsCoolTime) return;
            IsCoolTime = true;
            //Magic��Strength�̍X�V
            _status.WizardStrength = player.Model.Strength;
            //���@�̐�����SE�Đ�
            AudioManager.Instance.PlaySE(_status.ShotSound);
            ShotMagic shotMagic = (ShotMagic)_status.Magic;
            //�N�[���^�C����UI�\��
            WizardUI.UIManager.Instance.ShotDisplayCoolTime(_status, num);
            shotMagic.Create(_status, player.transform.position, player.Model.Direction);
            //�N�[���^�C������
            await UniTask.Delay(TimeSpan.FromSeconds(_status.CoolTime));
            IsCoolTime = false;
        }
    }
}
