using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardMagic;
using UnityEngine.UI;

namespace WizardUI {
    public class SetMagicButton : MonoBehaviour
    {
        [SerializeField] private MagicCreatorStatus magic;
        [SerializeField] private SelectSetMagic select;
        private Button button;
        private IntroduceUI introduce;

        void Start()
        {
            button = GetComponent<Button>();
            introduce = GetComponent<IntroduceUI>();
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySE(AudioType.button);
                select.SetMagic(magic);
            });
        }
    }
}
