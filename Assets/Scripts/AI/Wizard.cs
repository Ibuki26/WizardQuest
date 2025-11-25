using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Wizard : MonoBehaviour
{
    [SerializeField] private WizardModelData data;

    private WizardModel _model;
    private WizardView _view;
    private Rigidbody2D _rb2d;
    private FlagsController<WizardState> _stateCon;
    private WizardMovement _movement;
    private CharacterRaycaster _raycaster;

    private Vector3 startPoint;
    private int stageNum = 0;
    private WizardAgent agent;

    public WizardModel Model => _model;

    public int StageNum => stageNum;

    public WizardAgent Agent 
    { 
        get { return agent; }
        set { agent = value; }
    }

    public void Initialize()
    {
        _model = new WizardModel(data);
        _view = GetComponent<WizardView>();
        _rb2d = GetComponent<Rigidbody2D>();
        _stateCon = new FlagsController<WizardState>();
        _movement = GetComponent<WizardMovement>();
        _raycaster = GetComponent<CharacterRaycaster>();

        _view.ManualStart();
        _movement.Initialize(_model, _stateCon);
        _raycaster.ManualStart();


        startPoint = transform.position;
        stageNum = 0;
        //装備のステータス適応はとりあえず後で
    }

    public void ResetWizard()
    {
        _model.HitPoint.Value = _model.MaxHitPoint;
        _stateCon.SetState(default);
        _rb2d.velocity = Vector2.zero;
        transform.position = startPoint;
        _view.FlipX(1);
        stageNum = 0;
        //CharacterRaycasterクラスなどの制御クラスのリセットは後で
    }

    private void FixedUpdate()
    {
        //地面の接地判定
        _raycaster.ManualFixedUpdate();
        IsGround();
        IsCeiling();
        _model.PlayerVelocity = _movement.UpdateVelocity(_model.PlayerVelocity);
        _rb2d.velocity = _model.PlayerVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<StageArea>(out var stage))
        {
            if (stage.num > stageNum)
                agent.AddReward(0.1f);
            else
                agent.AddReward(-0.1f);
        }

        if (collision.gameObject.TryGetComponent<DropArea>(out _))
        {
            agent.AddReward(-0.1f);
        }

        if (collision.gameObject.TryGetComponent<GoalFlag>(out _))
        {
            agent.SetReward(1.0f);
            //エピソードの終了
            agent.EndEpisode();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<StageArea>(out var stage))
        {
            if (stage.num <= stageNum)
            {
                agent.AddReward(-0.001f);
            }
        }
    }

    public void OnMove(float xAxis)
    {
        _movement.XAxis = xAxis;

        //スティックに触っていないときの処理
        if (Mathf.Abs(xAxis) < 0.1f)
        {
            _view.SetAnimation("isRun", false);
            return;
        }

        //進行方向の記録と画像の向き調整
        _model.Direction = (xAxis > 0) ? 1 : -1;
        _view.FlipX(_model.Direction);

        agent.AddReward(0.001f);
        _view.SetAnimation("isRun", true);
    }

    public void OnJump(bool pushJump)
    {
        if (_stateCon.HasState(WizardState.Standing) && pushJump
            && !_stateCon.HasState(WizardState.Jumping))
        {
            //ジャンプを始める処理
            AudioManager.Instance.PlaySE(AudioType.jump);
            _view.SetAnimation("isJump", true);
            _model.PlayerVelocityY = _model.Jump;
            _stateCon.AddState(WizardState.Jumping);
            _movement.PushJumpButton = true;
        }
        if (!pushJump && _stateCon.HasState(WizardState.Jumping))
        {
            _movement.PushJumpButton = false;
            _view.SetAnimation("isJump", false);
            _stateCon.DeleteState(WizardState.Jumping);
        }
    }

    //地面と接しているかの判定
    private void IsGround()
    {
        //地面と接していたらStandingを追加
        //if (ground.IsGround(adjustRaycast_y))
        if (_raycaster.GetGrounded())
        {
            _stateCon.AddState(WizardState.Standing);

            if (_stateCon.HasState(WizardState.Jumping) && _model.PlayerVelocityY < 0.0f)
            {
                _view.SetAnimation("isJump", false);
                _stateCon.DeleteState(WizardState.Jumping);
            }
        }
        //地面から離れていたらStandingを削除
        else
            _stateCon.DeleteState(WizardState.Standing);
    }

    //天井と接しているかの判定
    private void IsCeiling()
    {
        if (_raycaster.GetGeilinged())
            _stateCon.AddState(WizardState.Ceiling);
        else
            _stateCon.DeleteState(WizardState.Ceiling);
    }
}