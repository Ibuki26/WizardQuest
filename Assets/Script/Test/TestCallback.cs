using UnityEngine;
using UnityEngine.InputSystem;

public class TestCallback : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;

    void OnEnable()
    {
        // ���ׂẴA�N�V�����}�b�v�𑖍�
        foreach (var map in inputActions.actionMaps)
        {
            map.Enable();

            // �e�A�N�V������ performed �R�[���o�b�N��ݒ�
            foreach (var action in map.actions)
            {
                action.performed += OnActionPerformed;
            }
        }
    }

    void OnDisable()
    {
        // �C�x���g�̉����ƃ}�b�v�̖�����
        foreach (var map in inputActions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.performed -= OnActionPerformed;
            }

            map.Disable();
        }
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        string actionName = context.action.name;
        string controlPath = context.control?.path ?? "Unknown";

        Debug.Log($"[Input Debug] �A�N�V����: {actionName}, �����ꂽ����: {controlPath}");
    }
}
