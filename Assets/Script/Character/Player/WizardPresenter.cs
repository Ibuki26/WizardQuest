using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.InputSystem;

public class WizardPresenter : MonoBehaviour
{
    [SerializeField] private int hitPoint;
    [SerializeField] private int strength;
    [SerializeField] private int defense;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private GameObject overWindow;

    private WizardModel _model;
    private WizardView _view;
    private WizardStateController stateCon;
    private GroundChecker ground;
    private MagicCreator[] magics = new MagicCreator[2];
    private PlayerInput _playerInput;
    private Rigidbody2D rb;

    private float gravity = -6; //��������y�����̑��x
    private float adjustRaycast_y = 0.005f; //Raycast������Ƃ��̔������W�̒����p���l
    private float jumpLimit = 0.7f; //�W�����v������E����

    public WizardModel Model => _model;
    public MagicCreator[] Magics => magics;

    private void SetM()
    {
        //���@�̃Z�b�g
        magics[0] = MagicCreator.Initialize(MyStatus.magics[0]);
        magics[1] = MagicCreator.Initialize(MyStatus.magics[1]);
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void ManualStart()
    {
        _model = new WizardModel(hitPoint, strength, defense, speed, jump);
        _view = GetComponent<WizardView>();
        rb = GetComponent<Rigidbody2D>();
        _view.ManualStart();
        stateCon = GetComponent<WizardStateController>();
        stateCon.SetState(WizardState.Standing);
        ground = GetComponent<GroundChecker>();

        //Magic�̎擾
        SetM();
        //�����̎擾
        //UI�ւ̏��̑��M�@���邩�s��
    }

    public void ManualFixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.7f, 0), Color.yellow, 1f);
        //�n�ʂ̐ڒn����
        IsGround();
        AdjustVelocity_y();
        rb.velocity = Model.PlayerVelocity;
    }

    //PlayerInput�ւ̊֐��̓o�^
    private void OnEnable()
    {
        if (_playerInput == null) return;

        _playerInput.onActionTriggered += OnMove;
        _playerInput.onActionTriggered += OnJump;
        _playerInput.onActionTriggered += OnMagic;
    }

    //PlayerInput�ւ̊֐��o�^�̉���
    private void OnDisable()
    {
        if (_playerInput == null) return;

        _playerInput.onActionTriggered -= OnMove;
        _playerInput.onActionTriggered -= OnJump;
        _playerInput.onActionTriggered -= OnMagic;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.name != "Move") return;

        //�X�e�B�b�N�ɐG���Ă��Ȃ��Ƃ��̏���
        if (context.canceled)
        {
            _view.SetAnimation("isRun", false);
            Model.PlayerVelocityX = 0f;
            return;
        }

        //x�������̓��͏��̎擾
        float xAxis = context.ReadValue<Vector2>().x;

        //�i�s�����̋L�^�Ɖ摜�̌�������
        _model.Direction = (xAxis > 0) ? 1 : -1;
        transform.localScale = new Vector3(0.35f * _model.Direction, 0.35f, 1);

        _view.SetAnimation("isRun", true);
        Model.PlayerVelocityX = Model.RunSpeed(Model.Speed, Model.Direction);
    }

    public async void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed
            || context.action.name != "Jump") return;

        if (stateCon.HasState(WizardState.Standing))
        {
            //�W�����v���n�߂鏈��
            AudioManager.Instance.PlaySE(AudioType.jump);
            _view.SetAnimation("isJump", true);
            Model.PlayerVelocityY = _model.Jump;
            stateCon.AddState(WizardState.Jumping);

            await UniTask.Delay(TimeSpan.FromSeconds(jumpLimit), cancellationToken: this.GetCancellationTokenOnDestroy());
            //�����鏈��
            _view.SetAnimation("isJump", false);
            stateCon.DeleteState(WizardState.Jumping);
            stateCon.AddState(WizardState.falling);
        }
    }

    private async void OnMagic(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed
            || (context.action.name != "Magic1" && context.action.name != "Magic2")) return;
        var magicNum = (context.action.name == "Magic1") ? 0 : 1; //���@1��2������

        //���@���s���ƃN�[���^�C�����̂Ƃ��͎��s���Ȃ�
        if (stateCon.HasState(WizardState.Magicing) || magics[magicNum].IsCoolTime) return;

        stateCon.AddState(WizardState.Magicing);
        _view.SetAnimTrigger("attack");

        //�A�j���[�V�����̃^�C�~���O����
        await UniTask.WaitUntil(() =>
            _view.StateInfo.IsName("Attack") &&
            _view.StateInfo.normalizedTime >= 0.9f
        , cancellationToken: this.GetCancellationTokenOnDestroy());

        magics[magicNum].CreateMagic(this, magicNum);
        //�ʂ̃{�^���̖��@���قړ����Ɏg���Ȃ��悤�ɑҋ@
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        stateCon.DeleteState(WizardState.Magicing);
    }

    public async UniTask DamageFromEnemy(int enemyAttack, int enemyStrength)
    {
        //�_���[�W������Ԓ��Ɩ��@�g�p���̓_���[�W���󂯂Ȃ�
        if (stateCon.HasState(WizardState.IgnoreDamage)
            || stateCon.HasState(WizardState.Magicing)) return;
        if (_model.HitPoint.Value == 0) return;

        //���G��Ԃ̊J�n
        stateCon.AddState(WizardState.IgnoreDamage);

        //�_���[�W�ʂ̌v�Z
        var damage = Model.CalculateDamage(enemyAttack, enemyStrength);
        //�̗�-�_���[�W�ʂ����̒l�ɂȂ�����0�A�����łȂ��Ȃ�̗�-�_���[�W�ʂ����̂܂ܑ��
        _model.HitPoint.Value = _model.DecreaseHitPoint(damage);
        Die();

        //�_���[�W���o
        AudioManager.Instance.PlaySE(AudioType.damage_player);
        _view.SetAnimTrigger("hurt");

        //���G��Ԃ̑ҋ@
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
        //���G��Ԃ̉���
        stateCon.DeleteState(WizardState.IgnoreDamage);
    }

    public async UniTask DamageFromGimmick(int damage)
    {
        //�_���[�W������Ԓ��Ɩ��@�g�p���̓_���[�W���󂯂Ȃ�
        if (stateCon.HasState(WizardState.IgnoreDamage)
            || stateCon.HasState(WizardState.Magicing)) return;
        if (_model.HitPoint.Value == 0) return;

        //���G��Ԃ̊J�n
        stateCon.AddState(WizardState.IgnoreDamage);

        _model.HitPoint.Value = _model.DecreaseHitPoint(damage);
        Die();

        //�_���[�W���o
        AudioManager.Instance.PlaySE(AudioType.damage_player);
        _view.SetAnimTrigger("hurt");

        //���G��Ԃ̑ҋ@
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
        //���G��Ԃ̉���
        stateCon.DeleteState(WizardState.IgnoreDamage);
    }

    public void Heal(int healPoint)
    {
        //���̗̑͂Ɖ񕜒l�𑫂��āA����l�𒴂��������l�����A�����łȂ��Ȃ�̗�+�񕜒l����
        _model.HitPoint.Value = Model.IncreaseHitPoint(healPoint);
    }

    private void Die()
    {
        if (_model.HitPoint.Value > 0) return;
        //PlayerInput�ւ̊֐��o�^�̉���
        _playerInput.onActionTriggered -= OnMove;
        _playerInput.onActionTriggered -= OnJump;
        _playerInput.onActionTriggered -= OnMagic;

        AudioManager.Instance.PlaySE(AudioType.gameOver);
        Model.PlayerVelocity = Vector2.zero;
        _view.SetAnimTrigger("die");
        //�Q�[���I�[�o�[��ʂ̕\��
        _playerInput.actions.Disable();
        _playerInput.actions.FindActionMap("UI");
        overWindow.SetActive(true);
    }

    //�n�ʂƐڂ��Ă��邩�̔���
    private void IsGround()
    {
        //�n�ʂƐڂ��Ă�����Standing��ǉ�
        if (ground.IsGround(adjustRaycast_y))
            stateCon.AddState(WizardState.Standing);
        //�n�ʂ��痣��Ă�����Standing���폜
        else
            stateCon.DeleteState(WizardState.Standing);
    }

    //�W�����v���ȊO�ł�y�����̑��x�̒���
    private void AdjustVelocity_y()
    {
        //�n�ʂƐڂ��Ă���Ƃ���y�����̑��x�����̒l�̂Ƃ��̏���
        if (rb.velocity.y < 0 && stateCon.HasState(WizardState.Standing))
        {
            Model.PlayerVelocityY = 0f;
            stateCon.DeleteState(WizardState.falling);
        }

        //�W�����v���ȊO�ŋ󒆂ɂ���Ƃ��̏���
        if (!stateCon.HasState(WizardState.Jumping) &&
            !stateCon.HasState(WizardState.Standing))
        {
            Model.PlayerVelocityY = gravity;
        }
    }
}
