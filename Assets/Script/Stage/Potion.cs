using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int healPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out var player))
        {
            AudioManager.Instance.PlaySE(AudioType.heal);
            player.Heal(healPoint);
            Destroy(gameObject, 0.1f);
        }
    }
}
