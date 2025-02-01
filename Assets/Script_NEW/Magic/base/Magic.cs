using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardPlayer;

namespace WizardMagic
{
    public abstract class Magic : MonoBehaviour
    {
        //Magic�����֐��̃C���^�[�t�F�[�X
        public interface CreateMethod
        {
            //shot�^�p
            void Create(ShotMagicCreatorStatus status, Vector3 playerPosition, int direction)
            {
                Debug.Log("shot�p��Create�ł��B");
            }

            //buff�^�p
            void Create(MagicCreatorStatus status, WizardPresenter player)
            {
                Debug.Log("buff�p��Create�ł��B");
            }

            //area�^�p
            GameObject Create(AreaMagicCreatorStatus status, WizardPresenter player)
            {
                Debug.Log("area�p��Create�ł��B");
                return null;
            }
        }
    }
}
