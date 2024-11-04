using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    [SerializeField] private Vector3 pos;
    [SerializeField] private MainCamera mainCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out var player)){
            player.transform.position = pos;
            mainCamera.Move(new Vector3(pos.x, 0, -10));
            AudioManager.Instance.PlaySE(AudioType.damage_player);
            player.Damage(10);
        }

        if(collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.Died();
        }
    }
}
