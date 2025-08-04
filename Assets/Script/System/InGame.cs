using UnityEngine;
using Cysharp.Threading.Tasks;

public class InGame : MonoBehaviour
{
    [SerializeField] private WizardPresenter _player;
    [SerializeField] private MagicCreatorStatus[] magic;
    [SerializeField] private EnemyPresenter[] enemy;

    void Start()
    {
        SkillManager.Instance.SetSkillBase(MyStatusManager.Instance.FetchEquipment());
        _player.ManualStart();
        for (int i = 0; i < enemy.Length; i++)
        {
            if(enemy[i] != null)
                enemy[i].ManualStart();
        }
        UIManager.Instance.ManualStart();
    }

    private void Update()
    {
        PlayDataRecorder.Instance.AddPlayTime(Time.deltaTime);
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
