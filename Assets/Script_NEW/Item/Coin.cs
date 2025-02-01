using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardPlayer;
using WizardUI;

namespace WizardItem
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int score;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent<WizardPresenter>(out var player))
            {
                AudioManager.Instance.PlaySE(AudioType.coin);
                WizardUI.UIManager.Instance.AddScore(score);
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
