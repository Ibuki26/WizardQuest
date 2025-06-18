using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour
{
    [SerializeField] private InputActionReference actionRef;
    private InputAction action;

    private void OnEnable()
    {
        action = actionRef.action;
        if (action == null) return;

        action.performed += OnBackTitle;

        action?.Enable();
    }

    private void OnDisable()
    {
        if (action == null) return;

        action.performed -= OnBackTitle;

        action?.Disable();
    }

    private void OnBackTitle(InputAction.CallbackContext context)
    {
        AudioManager.Instance.PlaySE(AudioType.backHome);
        SceneManager.LoadScene("Title");
    }
}
