using UnityEngine;
using UnityEngine.InputSystem;

public class GoalFlag : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ShowData data;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<WizardPresenter>(out _))
        {
            AudioManager.Instance.PlaySE(AudioType.gameClear);
            PlayDataRecorder.Instance.SaveDataToCSV_Multi();
            playerInput.actions.Disable();
            playerInput.actions.FindAction("UI");
            UIManager.Instance.ShowGameClear();
            data.Show();
        }
    }
}
