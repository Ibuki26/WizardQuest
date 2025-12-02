using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Cysharp.Threading.Tasks.Triggers;

public class WizardAgent : Agent
{
    [SerializeField] private Wizard wizard;

    private float smoothedXAxis = 0f;
    public float xAxisSmoothTime = 0.1f; // 慣性の大きさ
    [SerializeField] int jumpButtonPhase = 0; //ジャンプボタンの状態を整数で管理する。
    //0でボタンを押していない。1でボタンを押し始めたとき。2でボタンを押し続ける
    //private float highestPoint = 0f;
    //private float lastPositionX = 0f;
    //private float timeSinceLastMove = 0f;
    //private float timeStayLowNum = 0f;

    public override void Initialize()
    {
        wizard.Initialize();
        wizard.Agent = this;
        jumpButtonPhase = 0;
        //highestPoint = wizard.StartPoint.y;
        //lastPositionX = wizard.StartPoint.x;
        //timeSinceLastMove = 0f;
        //timeStayLowNum = 0f;
    }

    public override void OnEpisodeBegin()
    {
        var platforms = FindObjectsOfType<Scaffold>();
        foreach (var p in platforms)
        {
            p.touchChecker = false;
        }

        jumpButtonPhase = 0;
        //timeSinceLastMove = 0f;
        //timeStayLowNum = 0f;
        wizard.ResetWizard();
        //highestPoint = wizard.StartPoint.y;
        //lastPositionX = wizard.StartPoint.x;
    }

    /*
    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(wizard.CountDrop);
        //sensor.AddObservation(wizard.CountLowStage);
    }
    */

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 currentPosition = wizard.transform.position;

        if(currentPosition.y < -13.0f)
        {
            EndEpisode();
        }

        /*
        //最高到達点が高くなっていたら報酬
        if(currentPosition.y > highestPoint)
        {
            AddReward((currentPosition.y - highestPoint) * 0.01f);
            highestPoint = currentPosition.y;
        }
        */

        /*
        //横移動していない時間を計測
        if(Mathf.Abs(currentPosition.x - lastPositionX) < 0.01f)
        {
            timeSinceLastMove += Time.deltaTime;
        }
        else
        {
            timeSinceLastMove = 0f;
        }
        lastPositionX = currentPosition.x;

        //横移動していない時間が一定値を超えたら負の報酬
        if(timeSinceLastMove > 1.0f)
        {
            AddReward(-0.001f);
        }
        */

        //横移動の入力取得
        float raw = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);

        // 生の行動値 → 慣性付き入力へ
        smoothedXAxis = Mathf.MoveTowards(
            smoothedXAxis,
            raw,
            Time.fixedDeltaTime / xAxisSmoothTime
        );
        var xAxis = smoothedXAxis;
        
        //ジャンプの入力取得
        jumpButtonPhase = JumpButtonPhase(actions.DiscreteActions[0]);

        wizard.OnMove(xAxis);
        wizard.OnJump(jumpButtonPhase);
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
        discreteActionOut[0] = JumpButtonPhaseHeuristic();
    }

    private int JumpButtonPhase(int discreteAction)
    {
        switch (discreteAction)
        {
            case 0:
                if (jumpButtonPhase == 2)
                    return 0;
                else
                    return jumpButtonPhase;
            case 1:
                if(jumpButtonPhase == 0)
                    return 1;
                else
                    return jumpButtonPhase;
            case 2:
                if(jumpButtonPhase == 1)
                    return 2;
                else
                    return jumpButtonPhase;
            default:
                return jumpButtonPhase;
        }
    }

    private int JumpButtonPhaseHeuristic()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpButtonPhase == 2)
                return 0;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (jumpButtonPhase == 1)
                return 2;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpButtonPhase == 0)
                return 1;
        }
        
        return jumpButtonPhase;
    }
}