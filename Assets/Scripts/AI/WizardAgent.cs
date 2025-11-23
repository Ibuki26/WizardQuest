using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class WizardAgent : Agent
{
    [SerializeField] private Wizard wizard;

    private float smoothedXAxis = 0f;
    public float xAxisSmoothTime = 0.1f; // Šµ«‚Ì‘å‚«‚³

    public override void Initialize()
    {
        wizard.Initialize();
        wizard.Agent = this;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(wizard.StageNum);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float raw = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);

        // ¶‚Ìs“®’l ¨ Šµ«•t‚«“ü—Í‚Ö
        smoothedXAxis = Mathf.MoveTowards(
            smoothedXAxis,
            raw,
            Time.fixedDeltaTime / xAxisSmoothTime
        );

        var xAxis = smoothedXAxis;
        var pushJump = actions.DiscreteActions[0] > 0;

        wizard.OnMove(xAxis);
        wizard.OnJump(pushJump);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActionOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.D))
        {
            continousActionOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            continousActionOut[0] = -1;
        }

        var discreteActionOut = actionsOut.DiscreteActions;
        discreteActionOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}