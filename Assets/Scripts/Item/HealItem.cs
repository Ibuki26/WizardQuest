using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private int healPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<WizardPresenter>(out var player))
        {
            AudioManager.Instance.PlaySE(AudioType.heal);
            PlayDataRecorder.Instance.AddGetHaert();
            player.Heal(healPoint);
            Destroy(gameObject, 0.1f);
        }
    }
}
