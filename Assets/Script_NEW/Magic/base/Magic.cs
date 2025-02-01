using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardPlayer;

namespace WizardMagic
{
    public abstract class Magic : MonoBehaviour
    {
        //Magic生成関数のインターフェース
        public interface CreateMethod
        {
            //shot型用
            void Create(ShotMagicCreatorStatus status, Vector3 playerPosition, int direction)
            {
                Debug.Log("shot用のCreateです。");
            }

            //buff型用
            void Create(MagicCreatorStatus status, WizardPresenter player)
            {
                Debug.Log("buff用のCreateです。");
            }

            //area型用
            GameObject Create(AreaMagicCreatorStatus status, WizardPresenter player)
            {
                Debug.Log("area用のCreateです。");
                return null;
            }
        }
    }
}
