using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour
{
    [SerializeField] private InputAction action;

    private void OnEnable()
    {
        action.performed += OnStart;
        action?.Enable();
    }

    private void OnDisable()
    {
        action.performed -= OnStart;
        action?.Disable();
    }

    private void OnStart(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySE(AudioType.start);
        SceneManager.LoadScene("ModeSelect");
    }
}
