using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private AudioType type;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public async void OnButtonClick()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            var token = this.GetCancellationTokenOnDestroy();
            AudioManager.Instance.PlaySE(type);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not assigned!");
        }
    }

    public void OnCursorEnter()
    {
        AudioManager.Instance.PlaySE(AudioType.cursor);
    }
}
