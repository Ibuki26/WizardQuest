using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WindowSwitcherAction : MonoBehaviour
{
    [SerializeField] private GameObject[] windows;
    [SerializeField] private Button[] buttons;
    [SerializeField] private EventSystem eventSystem;
    private int num = 0;

    public void SwitchRight(InputAction.CallbackContext context)
    {
        //現在のウィンドウが1，2枚目のとき
        if(num != 2)
        {
            //前のウィンドウを非表示状態にし、新しいウィンドウを表示状態にする
            windows[num].SetActive(false);
            windows[++num].SetActive(true);
            //新しいウィンドウにあるボタンを選択状態にする
            eventSystem.SetSelectedGameObject(buttons[num].gameObject);
        }
        //現在のウィンドウが3枚目のとき
        else
        {
            //前のウィンドウを非表示状態にし、新しいウィンドウを表示状態にする
            windows[num].SetActive(false);
            num = 0;
            windows[num].SetActive(true);
            //新しいウィンドウにあるボタンを選択状態にする
            eventSystem.SetSelectedGameObject(buttons[num].gameObject);
        }
    }

    public void SwitchLeft(InputAction.CallbackContext context)
    {
        //現在のウィンドウが2，3枚目のとき
        if(num != 0)
        {
            //前のウィンドウを非表示状態にし、新しいウィンドウを表示状態にする
            windows[num].SetActive(false);
            windows[--num].SetActive(true);
            //新しいウィンドウにあるボタンを選択状態にする
            eventSystem.SetSelectedGameObject(buttons[num].gameObject);
        }
        //現在のウィンドウが1枚目のとき
        else
        {
            //前のウィンドウを非表示状態にし、新しいウィンドウを表示状態にする
            windows[num].SetActive(false);
            num = 2;
            windows[num].SetActive(true);
            //新しいウィンドウにあるボタンを選択状態にする
            eventSystem.SetSelectedGameObject(buttons[num].gameObject);
        }
    }
}
