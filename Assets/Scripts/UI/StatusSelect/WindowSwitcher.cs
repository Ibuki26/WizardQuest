using UnityEngine;
using UnityEngine.InputSystem;

public class WindowSwitcher : MonoBehaviour
{
    [SerializeField] private InputActionAsset asset;

    private WindowSwitcherAction switcherAction;
    private InputAction switchRightAction;
    private InputAction switchLeftAction;

    private void OnEnable()
    {
        switcherAction = GetComponent<WindowSwitcherAction>();
        //AssetからMapを取得
        var UImap = asset.FindActionMap("UI");
        //MapからActionを取得
        switchRightAction = UImap.FindAction("SwitchRIght");
        switchLeftAction = UImap.FindAction("SwitchLeft");
        //Actionへイベントの登録
        switchRightAction.performed += switcherAction.SwitchRight;
        switchLeftAction.performed += switcherAction.SwitchLeft;
        //アクションを有効化
        switchRightAction.Enable();
        switchLeftAction.Enable();
    }

    private void OnDisable()
    {
        //イベントの解除
        switchRightAction.performed -= switcherAction.SwitchRight;
        switchLeftAction.performed -= switcherAction.SwitchLeft;
        //アクションの無効化
        switchRightAction.Disable();
        switchLeftAction.Disable();
    }
}
