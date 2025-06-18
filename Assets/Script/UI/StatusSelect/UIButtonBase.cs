using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

//継承用。ゲーム内のボタンの基本的な処理
public class UIButtonBase : MonoBehaviour
{
    protected Button button;
    private bool hasSubmitted = false; //1回の入力で複数回押すのを防ぐため

    //ボタンによって引数の有無があるためAddLisnterは継承先で行う
    protected virtual void Start()
    {
        button = GetComponent<Button>();
    }

    //ボタンを押したときの全体の処理 引数なし
    protected async UniTask HandleSubmitAsync(Action action)
    {
        if (hasSubmitted) return;
        hasSubmitted = true;

        action?.Invoke();
        
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        hasSubmitted = false;
    }

    //ボタンを押したときの全体の処理 引数あり
    protected async UniTask HandleSubmitAsync<T>(Action<T> action, T arg)
    {
        if (hasSubmitted) return;
        hasSubmitted = true;

        action?.Invoke(arg);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        hasSubmitted = false;
    }
}
