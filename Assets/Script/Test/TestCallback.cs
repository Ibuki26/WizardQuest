using UnityEngine;
using UnityEngine.InputSystem;

public class TestCallback : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;

    void OnEnable()
    {
        // すべてのアクションマップを走査
        foreach (var map in inputActions.actionMaps)
        {
            map.Enable();

            // 各アクションに performed コールバックを設定
            foreach (var action in map.actions)
            {
                action.performed += OnActionPerformed;
            }
        }
    }

    void OnDisable()
    {
        // イベントの解除とマップの無効化
        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.performed -= OnActionPerformed;
            }

            map.Disable();
        }
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        string actionName = context.action.name;
        string controlPath = context.control?.path ?? "Unknown";

        Debug.Log($"[Input Debug] アクション: {actionName}, 押された入力: {controlPath}");
    }
}
