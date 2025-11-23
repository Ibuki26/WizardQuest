using UnityEngine;

public class WizardMovement : MonoBehaviour
{
    //Horizontalの入力を記録する変数
    private float xAxis = 0f;
    //地面を移動するときx方向の加速、減速時の加速度
    private float groundAcceleration = 20f;
    private float groundDeceleration = 20f;
    //空中にいるときのx方向の加速、減速時の加速度
    private float airborneAccelProportion = 1.0f;
    private float airborneDecelProportion = 0.2f;
    //ジャンプボタンが押されているか記録する変数
    private bool pushJumpButton = false;
    //ジャンプボタンを押さなくったときに減算される数値
    private float jumpAbortSpeedReduction = 10;
    //重力
    private float gravity = 12f;

    private WizardModel wizardModel;
    private FlagsController<WizardState> wizardStateCon;

    public float XAxis
    {
        get { return xAxis; }
        set { xAxis = value; }
    }

    public bool PushJumpButton
    {
        get { return pushJumpButton; }
        set { pushJumpButton = value; }
    }

    public void Initialize(WizardModel model, FlagsController<WizardState> stateCon)
    {
        wizardModel = model;
        wizardStateCon = stateCon;
    }

    public Vector2 UpdateVelocity(Vector2 velocity)
    {
        float nextSpeed_x;
        float nextSpeed_y;

        //プレイヤーが地上にいるときの処理
        if (wizardStateCon.HasState(WizardState.Standing))
        {
            nextSpeed_x = GroundedHorizontalMovement(velocity.x);
            nextSpeed_y = GroundedVerticalMovement(velocity.y);
        }
        //プレイヤーが空中にいるときの処理
        else
        {
            nextSpeed_x = AirborneHorizontalMovement(velocity.x);
            nextSpeed_y = AirborneVerticalMovement(velocity.y) + UpdateJump(velocity.y);;
        }

        return new Vector2(nextSpeed_x, nextSpeed_y);
    }

    //地面の上を移動するときのx方向の速度の計算を行う関数
    public float GroundedHorizontalMovement(float currentSpeed_x)
    {
        float maxSpeed = wizardModel.GetRunSpeed();
        float desiredSpeed = xAxis != 0f ? maxSpeed: 0f;
        float acceleration = xAxis != 0f ? groundAcceleration : groundDeceleration;
        return Mathf.MoveTowards(currentSpeed_x, desiredSpeed, acceleration * Time.fixedDeltaTime);
    }

    private float GroundedVerticalMovement(float currentSpeed_y)
    {
        if (currentSpeed_y < -gravity * Time.fixedDeltaTime)
            return -gravity * Time.fixedDeltaTime;

        return currentSpeed_y - gravity * Time.fixedDeltaTime;
    }

    //空中で移動するときのx方向の速度の計算を行う関数
    private float AirborneHorizontalMovement(float currentSpeed_x)
    {
        float maxSpeed = wizardModel.GetRunSpeed();
        float desiredSpeed = xAxis != 0f ? maxSpeed : 0f;
        float acceleration = xAxis != 0f ? groundAcceleration * airborneAccelProportion : groundDeceleration * airborneDecelProportion;
        return Mathf.MoveTowards(currentSpeed_x, desiredSpeed, acceleration * Time.fixedDeltaTime);
    }

    private float AirborneVerticalMovement(float currentSpeed_y)
    {
        //浮動小数点の誤差でほぼ止まってるけど落下に移らない状態を防ぐため
        if (Mathf.Approximately(currentSpeed_y, 0f) && currentSpeed_y > 0.0f)
            return 0f;

        return currentSpeed_y - gravity * Time.fixedDeltaTime;
    }

    private float UpdateJump(float currentSpeed_y)
    {
        bool isCeiling = wizardStateCon.HasState(WizardState.Ceiling);
        if (!pushJumpButton || isCeiling && currentSpeed_y > 0.0f)
        {
            return - jumpAbortSpeedReduction * Time.fixedDeltaTime;
        }

        return 0;
    }
}
