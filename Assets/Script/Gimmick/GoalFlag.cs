using UnityEngine;
using UnityEngine.InputSystem;

public class GoalFlag : MonoBehaviour
{
    [SerializeField] private GameObject clearWindow;
    [SerializeField] private PlayerInput playerInput;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<WizardPresenter>(out _))
        {
            AudioManager.Instance.PlaySE(AudioType.gameClear);
            playerInput.actions.Disable();
            playerInput.actions.FindAction("UI");
            clearWindow.SetActive(true);
        }
    }
}
