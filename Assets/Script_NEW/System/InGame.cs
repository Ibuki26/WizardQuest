using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using WizardPlayer;
using WizardMagic;
using WizardEnemy;
using WizardUI;

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
        WizardUI.UIManager.Instance.ManualStart();
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
