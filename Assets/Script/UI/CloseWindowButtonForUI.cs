using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseWindowButtonForUI : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(AudioType.closeWindow);
            transform.parent.gameObject.SetActive(false);
        });
    }

    public void OnCursorEnter()
    {
        AudioManager.Instance.PlaySE(AudioType.cursor);
    }
}
