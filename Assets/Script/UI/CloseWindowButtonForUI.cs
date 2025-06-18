using UnityEngine;
using UnityEngine.UI;

public class CloseWindowButtonForUI : MonoBehaviour
{
    [SerializeField] private ModeSelectButtonOperator modeOperator;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySE(AudioType.closeWindow);
            transform.parent.gameObject.SetActive(false);
            modeOperator.enabled = true;
        });
    }
}
