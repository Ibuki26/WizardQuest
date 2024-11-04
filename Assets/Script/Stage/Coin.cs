using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private UIManager ui;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out var player))
        {
            AudioManager.Instance.PlaySE(AudioType.coin);
            ui.AddScore(score);
            Destroy(gameObject, 0.1f);
        }
    }
}
