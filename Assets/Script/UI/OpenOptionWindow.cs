using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenOptionWindow : MonoBehaviour
{
    [SerializeField] private InputActionReference actionRef;
    [SerializeField] private GameObject option;
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
        option.SetActive(true);
    }
}
