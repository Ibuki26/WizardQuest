using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GoHome : MonoBehaviour
{
    [SerializeField] private InputActionReference actionRef;
    private InputAction action;

    private void OnEnable()
    {
        if (actionRef == null) return;
        action = actionRef.action;
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
