using UnityEngine;

public class InGame : MonoBehaviour
{
    [SerializeField] private WizardPresenter _player;
    [SerializeField] private MagicCreatorStatus[] magic;
    [SerializeField] private EnemyPresenter[] enemy;

    void Start()
    {
        _player.ManualStart();
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].ManualStart();
        }
        UIManager.Instance.ManualStart();
    }

    void FixedUpdate()
    {
        _player.ManualFixedUpdate();
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i] != null)
                enemy[i].ManualFixedUpdate();
        }
    }
}
