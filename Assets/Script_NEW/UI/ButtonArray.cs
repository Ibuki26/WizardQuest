using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WizardUI
{
    public class ButtonArray : MonoBehaviour
    {
        [SerializeField] private GameObject[] buttons;
        private GameObject selectedButton;

        void OnEnable()
        {
            selectedButton = buttons[0];
            EventSystem.current.SetSelectedGameObject(buttons[0]);
        }

        /*private void OnDisable()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }*/

        public void SelectButton(int selectNum)
        {
            if (buttons[selectNum] == null) return;
            EventSystem.current.SetSelectedGameObject(null);
            selectedButton = buttons[selectNum];
            EventSystem.current.SetSelectedGameObject(buttons[selectNum]);

            if(buttons[selectNum].TryGetComponent<IntroduceUI>(out var introduce))
            {
                introduce.ChangeUI();
            }
        }

        public void Click()
        {
            if (selectedButton == null) return;
            var select = selectedButton.GetComponent<Button>();
            select.onClick.Invoke();
        }
    }
}
