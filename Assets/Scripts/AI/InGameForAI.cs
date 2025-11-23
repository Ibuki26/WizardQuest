using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameForAI : MonoBehaviour
{
    [SerializeField] private Wizard wizard;
    [SerializeField] private MagicCreatorStatus[] magic;
    [SerializeField] private EnemyPresenter[] enemy;

    void Start()
    {
        /*SkillManager.Instance.SetSkillBase(MyStatusManager.Instance.FetchEquipment());
        _player.ManualStart();
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i] != null)
                enemy[i].ManualStart();
        }
        */
        UIManager.Instance.ManualStart(wizard);
    }

    /*
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
    */
}
