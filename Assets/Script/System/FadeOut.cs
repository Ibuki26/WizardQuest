using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Start()
    {
        FadeIn().Forget();
    }

    private async UniTaskVoid FadeIn()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            await UniTask.Yield();
        }

        fadeImage.fillAmount = 0f; // 完全にフェードインが完了
    }
}
