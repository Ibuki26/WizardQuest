using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class RollerAgent : Agent
{
    public Transform target; //TargetのTransform
    Rigidbody rBody; //RollerAgentのRigidbody

    //ゲームオブジェクト生成時に呼ばれる
    public override void Initialize()
    {
        //RollerAgentのRigidBodyの参照の取得
        this.rBody = GetComponent<Rigidbody>();
    }

    //エピソード開始時に呼ばれる
    public override void OnEpisodeBegin()
    {
        //RollerAgentが床から落下している時
        if(this.transform.localPosition.y < 0)
        {
            //RollerAgentの位置と速度をリセット
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);

            //Targetの位置のリセット
            target.localPosition = new Vector3(
                Random.value*8-4, 0.5f, Random.value*8-4);
        }
    }

    //観察取得時に呼ばれる
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition.x); //Targetのx座標
        sensor.AddObservation(target.localPosition.z); //Targetのz座標
        sensor.AddObservation(this.transform.localPosition.x); //Rolleragentのx座標
        sensor.AddObservation(this.transform.localPosition.z); //Rolleragentのz座標
        sensor.AddObservation(rBody.velocity.x); //RollerAgentのx速度
        sensor.AddObservation(rBody.velocity.z); //RollerAgentのz速度
    }

    //行動決定時に呼ばれる
    public override void OnActionReceived(ActionBuffers actions)
    {
        //RollerAgentに力を加える
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.z = actions.ContinuousActions[1];
        rBody.AddForce(controlSignal * 10);

        //RollAgentがTargetの位置にたどり着いた時
        float distanceToTarget = Vector3.Distance(
            this.transform.localPosition, target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        //RollerAgentが床から落下した時
        if(this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }
}
