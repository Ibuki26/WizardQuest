using UnityEngine;
using UnityEngine.InputSystem;

public class OpenOptionWindow : MonoBehaviour
{
    [SerializeField] private InputActionReference actionRef;
    [SerializeField] private GameObject option;
    [SerializeField] private ModeSelectButtonOperator modeOperator;
    private InputAction action;

    private void OnEnable()
    {
        action = actionRef.action;
        if (action == null) return;

        action.performed += OnOpenOption;

        action?.Enable();
    }

    private void OnDisable()
    {
        if (action == null) return;

        action.performed -= OnOpenOption;

        action?.Disable();
    }

    private void OnOpenOption(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySE(AudioType.openWindow);
        modeOperator.enabled = false;
        option.SetActive(true);
    }
}