using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WizardUI
{
    public class ScoreView : MonoBehaviour
    {
        private TextMeshProUGUI textMesh;

        public void ManualStart()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
        }

        //���_�e�L�X�g�̕\���X�V
        public void UpdateText(ScoreModel model)
        {
            textMesh.text = model.Score.ToString();
        }
    }
}
