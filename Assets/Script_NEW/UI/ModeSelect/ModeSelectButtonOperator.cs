using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

namespace WizardUI {
    public class ModeSelectButtonOperator : MonoBehaviour
    {
        [SerializeField] private ButtonArray array;
        [SerializeField] private InputActionReference actionRef;
        [SerializeField] private InputActionReference actionRef2;
        private InputAction action;
        private InputAction action2;
        private int selectNum = 0;

        private void OnEnable()
        {
            action = actionRef.action;
            action2 = actionRef2.action;

            if (action == null || action2 == null) return;

            action.started += OnSelect;
            action2.started += OnClickButton;

            action?.Enable();
            action2?.Enable();
        }

        private void OnDisable()
        {
            if (action == null || action2 == null) return;

            action.started -= OnSelect;
            action2.started -= OnClickButton;

            action?.Disable();
            action2.Disable();
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            if (context.canceled) return;

            var stickValue = context.ReadValue<Vector2>().y;
            if (stickValue == 0) return;
            //スティックを上に動かしたときの処理
            if(stickValue > 0)
            {
                if (selectNum == 0) return;

                selectNum = 0;
            }
            //スティックを下に動かしたときの処理
            else if(stickValue < 0)
            {
                if (selectNum == 1) return;
 
                selectNum = 1;
            }

            AudioManager.Instance.PlaySE(AudioType.cursor);
            array.SelectButton(selectNum);
        }

        private void OnClickButton(InputAction.CallbackContext context)
        {
            array.Click();
        }
    }
}
