using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButton : MonoBehaviour
{
    [SerializeField] private SetStatus status;
    [SerializeField] private int buttonNum;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(AudioType.button);
            status.SetNumber(buttonNum);
        });
    }

    public void OnCursorEnter()
    {
        AudioManager.Instance.PlaySE(AudioType.cursor);
    }
}
