using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardMagic
{
    public abstract class Magic : MonoBehaviour
    {
        private int attack;
        public int Attack => attack;

        //���@�̐����ƌ����A�З͂̐ݒ�
        public void Create(MagicCreatorStatus status)
        {
            Instantiate(gameObject, transform.position + status.AdjustPos, Quaternion.identity);
            attack = status.Attack;
            var magicScale = status.transform.localScale;
            transform.localScale = new Vector3(magicScale.x * status.Direction, magicScale.y, magicScale.z);
            Action(status.Speed * status.Direction, status.DisappearTime);
        }

        //������̖��@�̋���
        protected abstract void Action(float speed, float disappearTime);

        //�G�ɓ��������Ƃ��̏���
        protected abstract void Effect(Enemy enemy);

        //�v���C���[�ɓ��������Ƃ��̏���
        protected abstract void Buff(Player player);
    }
}
