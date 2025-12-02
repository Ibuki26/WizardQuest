using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayReward : MonoBehaviour
{
    [SerializeField] private Agent agent;
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        textMeshPro.SetText(agent.GetCumulativeReward().ToString());
    }
}
