using UnityEngine;
using UnityEngine.InputSystem;

public class ClearButtonOperator : MonoBehaviour
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

        var stickValue = context.ReadValue<Vector2>().x;
        if (stickValue == 0) return;
        //スティックを右に動かしたときの処理
        if (stickValue > 0)
        {
            if (selectNum == 1) return;

            selectNum = 1;
        }
        //スティックを左に動かしたときの処理
        else if (stickValue < 0)
        {
            if (selectNum == 0) return;

            selectNum = 0;
        }

        AudioManager.Instance.PlaySE(AudioType.cursor);
        array.SelectButton(selectNum);
    }

    private void OnClickButton(InputAction.CallbackContext context)
    {
        array.Click();
    }
}