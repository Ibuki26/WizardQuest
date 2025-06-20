using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectSetMagic : MonoBehaviour
{
    [SerializeField] private Image[] images = new Image[2];
    [SerializeField] private Image[] magicIcon;
    [SerializeField] private InputActionReference actionRef;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int selectNum = 0;
    private InputAction action;
    public int SelectNum => selectNum;

    private void Start()
    {
        magicIcon[0].sprite = MyStatus.Instance.magics[0].Image;
        magicIcon[1].sprite = MyStatus.Instance.magics[1].Image;
    }

    private void OnEnable()
    {
        if (actionRef == null) return;
        action = actionRef.action;
        action.performed += OnSelect;
        action?.Enable();
    }

    private void OnDisable()
    {
        if (action == null) return;

        action.performed -= OnSelect;
        action?.Disable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        selectNum = (selectNum == 1) ? 0 : 1;

        images[1 - selectNum].sprite = sprites[1];
        images[selectNum].sprite = sprites[0];
    }

    public void SetMagic(MagicCreatorStatus status)
    {
        MyStatus.Instance.magics[selectNum] = status;
        magicIcon[selectNum].sprite = status.Image;
    }
}
