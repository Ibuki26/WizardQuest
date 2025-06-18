using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class UIButtonParameterBase : MonoBehaviour
{
    protected Button button;
    private bool hasSubmitted = false; //1��̓��͂ŕ����񉟂��̂�h������

    //�{�^���ɂ���Ĉ����̗L�������邽��AddLisnter�͌p����ōs��
    protected virtual void Start()
    {
        button = GetComponent<Button>();
    }

    //�{�^�����������Ƃ��̑S�̂̏����@��������
    protected async UniTask HandleSubmitAsync<T>(Action<T> action, T arg)
    {
        if (hasSubmitted) return;
        hasSubmitted = true;

        action?.Invoke(arg);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f),
            cancellationToken: this.GetCancellationTokenOnDestroy());
        hasSubmitted = false;
    }

    //�I�[�o�[���C�h�p�B�{�^�����Ƃׂ̍�������
    protected virtual void PerformAction<T>(T arg) { }
}
