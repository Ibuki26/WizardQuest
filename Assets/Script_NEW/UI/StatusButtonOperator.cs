using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WizardUI
{
    public class StatusButtonOperator : MonoBehaviour
    {
        [SerializeField] private ButtonArray array;
        [SerializeField] private InputActionReference actionRef;
        [SerializeField] private InputActionReference actionRef2;
        private InputAction action;
        private InputAction action2;
        [SerializeField] private int selectNum;

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

            var vector2Value_first = context.ReadValue<Vector2>();
            var vector2Value = new Vector2(Mathf.Abs(vector2Value_first.x), Mathf.Abs(vector2Value_first.y));

            //ç∂âEÇÃèàóù
            if(vector2Value.x > vector2Value.y)
            {
                //âEë§
                if(vector2Value_first.x > 0)
                {
                    if (selectNum == 2 || selectNum == 5)
                    {
                        Debug.Log("right max");
                        return;
                    }

                    selectNum = selectNum + 1;
                }
                //ç∂ë§
                else if(vector2Value_first.x < 0)
                {
                    if (selectNum % 3 == 0)
                    {
                        Debug.Log("left ,max");
                        return;
                    }

                    selectNum = selectNum - 1;
                }
            }
            //è„â∫ÇÃèàóù
            else if(vector2Value.x < vector2Value.y)
            {
                //è„ë§
                if(vector2Value_first.y > 0)
                {
                    if (selectNum < 3)
                    {
                        Debug.Log("up max");
                        return;
                    }

                    selectNum = selectNum - 3;
                }
                //â∫ë§
                else if(vector2Value_first.y < 0)
                {
                    if (selectNum > 3)
                    {
                        Debug.Log("down max");
                        return;
                    }

                    selectNum = selectNum + 3;
                }
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
