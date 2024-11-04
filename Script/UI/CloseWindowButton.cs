using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CloseWindowButton : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIManager ui;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(AudioType.closeWindow);
            playerInput.SwitchCurrentActionMap("Player");
            ui.DisappearWindow("mouse");
            ui.DisappearWindow("option");
        });
    }
}
