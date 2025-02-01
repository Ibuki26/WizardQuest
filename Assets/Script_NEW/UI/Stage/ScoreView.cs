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

        //得点テキストの表示更新
        public void UpdateText(ScoreModel model)
        {
            textMesh.text = model.Score.ToString();
        }
    }
}
