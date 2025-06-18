using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

//Scene間の移動を行うクラス
public class LoadSceneButton : UIButtonBase
{
    [SerializeField] private string sceneName; //移動するファイルの名前

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => HandleSubmitAsync(PerformAction, sceneName).Forget());
    }

    protected void PerformAction(string text)
    {
        //指定したシーン名があるかの確認
        if (!string.IsNullOrEmpty(text))
        {
            WaitSEToScene(text).Forget();
        }
        //シーン名が無かった場合、報告
        else
        {
            Debug.LogWarning("Scene name is not assigned!");
        }
    }

    //UniTaskを使うため関数にし、仕様するときはForget関数をつける
    private async UniTask WaitSEToScene(string sceneName)
    {
        InputActionAssetController.Instance.DisableAsset();
        AudioManager.Instance.PlaySE(AudioType.button);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f)
            , cancellationToken: this.GetCancellationTokenOnDestroy());
        SceneManager.LoadScene(sceneName);
    }
}
