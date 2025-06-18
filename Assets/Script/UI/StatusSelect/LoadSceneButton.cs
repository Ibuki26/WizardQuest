using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;

//Scene�Ԃ̈ړ����s���N���X
public class LoadSceneButton : UIButtonBase
{
    [SerializeField] private string sceneName; //�ړ�����t�@�C���̖��O

    protected override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => HandleSubmitAsync(PerformAction, sceneName).Forget());
    }

    protected void PerformAction(string text)
    {
        //�w�肵���V�[���������邩�̊m�F
        if (!string.IsNullOrEmpty(text))
        {
            WaitSEToScene(text).Forget();
        }
        //�V�[���������������ꍇ�A��
        else
        {
            Debug.LogWarning("Scene name is not assigned!");
        }
    }

    //UniTask���g�����ߊ֐��ɂ��A�d�l����Ƃ���Forget�֐�������
    private async UniTask WaitSEToScene(string sceneName)
    {
        InputActionAssetController.Instance.DisableAsset();
        AudioManager.Instance.PlaySE(AudioType.button);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f)
            , cancellationToken: this.GetCancellationTokenOnDestroy());
        SceneManager.LoadScene(sceneName);
    }
}
