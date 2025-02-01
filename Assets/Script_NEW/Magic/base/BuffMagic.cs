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
            //Wizard�̎q�I�u�W�F�N�g�ɂ��A�G�t�F�N�g��Wizard���S�ɔ�������悤�ɂ���
            createdMagic.transform.parent = player.transform;
            //���@�̌����̐ݒ�
            var magicScale = transform.localScale;
            createdMagic.transform.localScale = new Vector3(magicScale.x * player.Model.Direction, magicScale.y, magicScale.z);
            createdMagic.Buff(player, destroyTime);
        }

        //Buff�̌���
        public abstract UniTask Buff(WizardPresenter player, float destroyTime);

        //���ʉ������̏���
        public abstract void Deactivate(WizardPresenter player);
    }
}
