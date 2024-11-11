using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace WizardMagic
{
    public class MagicCreator
    {
        private MagicCreatorStatus _status;
        private bool isCoolTime = false;

        #region getter,setter
        public MagicCreatorStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public bool IsCoolTime
        {
            get { return isCoolTime; }
            set { isCoolTime = value; }
        }
        #endregion

        public MagicCreator(MagicCreatorStatus status)
        {
            _status = status;
        }

        public async void CreateMagic()
        {
            if (isCoolTime) return;
            //���@�̐�����SE�Đ�
            AudioManager.Instance.PlaySE(_status.ShotSound);
            _status.Magic.Create(_status);
            //�N�[���^�C������
            isCoolTime = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_status.DisappearTime));
            isCoolTime = false;
        }
    }
}
