using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameClearFlag : MonoBehaviour
{
    [SerializeField] private UIManager ui;
    [SerializeField] private PlayerInput playerInput;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent<Player>(out var player))
        {
            AudioManager.Instance.PlaySE(AudioType.gameClear);
            playerInput.SwitchCurrentActionMap("UI");
            ui.ActiveWindow("mouse");
            ui.ActiveWindow("clear");
        }
    }
}
