using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.InputSystem;
using System.Threading;
using WizardMagic;

namespace WizardPlayer
{
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
        private WizardControlState currentState;
        private MagicCreator[] magics = new MagicCreator[2];
        private PlayerInput _playerInput;
        private Rigidbody2D rb;
        private RaycastHit2D hit;
        private CancellationTokenSource jump_cts; //OnJump�Ŏg�p����await�̃L�����Z���p�g�[�N��

        private float gravity = -4; //��������y�����̑��x
        private float adjustRaycast_y = 0.005f; //Raycast������Ƃ��̔������W�̒����p���l
        private float jumpLimit = 0.7f; //�W�����v������E����
        private float standardSpeed = 4.0f; //x�����̈ړ����x�̊�l
        private Vector2 playerVelocity = Vector2.zero; //�v���C���[�̈ړ����x���L�^����ϐ�

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
            currentState = WizardControlState.Standing;

            //Magic�̎擾
            SetM();
            //�����̎擾
            //UI�ւ̏��̑��M�@���邩�s��
        }

        public void ManualFixedUpdate()
        {
            //�n�ʂ̐ڒn����
            IsGround();
            AdjustVelocity_y();
            rb.velocity = playerVelocity;
        }

        private void OnEnable()
        {
            if (_playerInput == null) return;

            _playerInput.onActionTriggered += OnMove;
            _playerInput.onActionTriggered += OnJump;
            _playerInput.onActionTriggered += OnMagic1;
            _playerInput.onActionTriggered += OnMagic2;
        }

        private void OnDisable()
        {
            if (_playerInput == null) return;

            _playerInput.onActionTriggered -= OnMove;
            _playerInput.onActionTriggered -= OnJump;
            _playerInput.onActionTriggered -= OnMagic1;
            _playerInput.onActionTriggered -= OnMagic2;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.action.name != "Move") return;
            if (_model.HitPoint.Value == 0) return; //�̗͂�0�̂Ƃ��͓����Ȃ�

            //�X�e�B�b�N�ɐG���Ă��Ȃ��Ƃ��̏���
            if (context.canceled)
            {
                _view.SetAnimation("isRun", false);
                playerVelocity.x = 0f;
                return;
            }

            //x�������̓��͏��̎擾
            float xAxis = context.ReadValue<Vector2>().x;

            if (xAxis == 0) return;
            //�i�s�����̋L�^�Ɖ摜�̌�������
            _model.Direction = (xAxis > 0) ? 1 : -1;
            transform.localScale = new Vector3(0.35f * _model.Direction, 0.35f, 1);

            _view.SetAnimation("isRun", true);
            playerVelocity.x = standardSpeed * _model.Speed/100 * _model.Direction;
        }

        public async void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.name != "Jump") return;
            if (_model.HitPoint.Value == 0) return; //�̗͂�0�̂Ƃ��͓����Ȃ�

            jump_cts?.Cancel();
            jump_cts = new CancellationTokenSource();

            if (context.canceled && currentState.HasFlag(WizardControlState.Jumping)){
                //����if����Delay���L�����Z�����ė����鏈���Ɉڍs������
                jump_cts?.Cancel();

                return;
            }
            
            if (!context.performed) return;

            if (currentState.HasFlag(WizardControlState.Standing)){
                //�W�����v���n�߂鏈��
                AudioManager.Instance.PlaySE(AudioType.jump);
                _view.SetAnimation("isJump", true);
                playerVelocity.y = _model.Jump;
                currentState |= WizardControlState.Jumping;
                
                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(jumpLimit), cancellationToken: jump_cts.Token);
                }
                catch (OperationCanceledException)
                {

                }
                finally
                {
                    //�����鏈��
                    var token = this.GetCancellationTokenOnDestroy();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: token);
                    _view.SetAnimation("isJump", false);
                    currentState &= ~WizardControlState.Jumping;
                    currentState |= WizardControlState.falling;
                }
                
                return;
            }
        }

        private async void OnMagic1(InputAction.CallbackContext context)
        {
            if (context.action.name != "Magic1") return;
            if (!context.performed) return;
            //���@���s���ƃN�[���^�C�����̂Ƃ��͎��s���Ȃ�
            if (currentState.HasFlag(WizardControlState.Magicing) || magics[0].IsCoolTime) return;
            if (_model.HitPoint.Value == 0) return; //�̗͂�0�̂Ƃ��͓����Ȃ�

            currentState |= WizardControlState.Magicing;
            _view.SetAnimTrigger("attack");
            �@
            //�A�j���[�V�����̃^�C�~���O����
            await UniTask.WaitUntil(() =>
                _view.StateInfo.IsName("Attack") &&
                _view.StateInfo.normalizedTime >= 0.9f
            , cancellationToken : this.GetCancellationTokenOnDestroy());

            magics[0].CreateMagic(this, 0);
            //�ʂ̃{�^���̖��@���قړ����Ɏg���Ȃ��悤�ɑҋ@
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            currentState &= ~WizardControlState.Magicing;
        }

        private async void OnMagic2(InputAction.CallbackContext context)
        {
            if (context.action.name != "Magic2") return;
            if (!context.performed) return;
            //���@���s���ƃN�[���^�C�����̂Ƃ��͎��s���Ȃ�
            if (currentState.HasFlag(WizardControlState.Magicing) || magics[1].IsCoolTime) return;
            if (_model.HitPoint.Value == 0) return; //�̗͂�0�̂Ƃ��͓����Ȃ�

            currentState |= WizardControlState.Magicing;
            _view.SetAnimTrigger("attack");

            //�A�j���[�V�����̃^�C�~���O����
            await UniTask.WaitUntil(() =>
                _view.StateInfo.IsName("Attack") &&
                _view.StateInfo.normalizedTime >= 0.9f
            , cancellationToken : this.GetCancellationTokenOnDestroy());

            magics[1].CreateMagic(this, 1);
            //�ʂ̃{�^���̖��@���قړ����Ɏg���Ȃ��悤�ɑҋ@
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            currentState &= ~WizardControlState.Magicing;
        }

        public async UniTask Damage(int enemyAttack, int enemyStrength)
        {
            if (currentState.HasFlag(WizardControlState.IgnoreDamage)
                || currentState.HasFlag(WizardControlState.Magicing)) return;
            if (_model.HitPoint.Value == 0) return;

            //���G��Ԃ̊J�n
            currentState |= WizardControlState.IgnoreDamage;
           
            //�_���[�W�ʂ̌v�Z
            var resultDamage = enemyAttack + enemyStrength / 5 - _model.Defense/10;
            //�̗�-�_���[�W�ʂ����̒l�ɂȂ�����0�A�����łȂ��Ȃ�̗�-�_���[�W�ʂ����̂܂ܑ��
            _model.HitPoint.Value = (_model.HitPoint.Value - resultDamage < 0) ? 0 : _model.HitPoint.Value - resultDamage;
            Die();
            
            //�_���[�W���o
            AudioManager.Instance.PlaySE(AudioType.damage_player);
            _view.SetAnimTrigger("hurt");
            
            //���G��Ԃ̑ҋ@
            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            //���G��Ԃ̉���
            currentState &= ~WizardControlState.IgnoreDamage;
        }

        public void Heal(int healPoint)
        {
            //���̗̑͂Ɖ񕜒l�𑫂��āA����l�𒴂��������l�����A�����łȂ��Ȃ�̗�+�񕜒l����
            _model.HitPoint.Value = (_model.HitPoint.Value + healPoint > _model.MaxHitPoint) ? _model.MaxHitPoint : _model.HitPoint.Value + healPoint;
        }

        private void Die()
        {
            if (_model.HitPoint.Value > 0) return;

            AudioManager.Instance.PlaySE(AudioType.gameOver);
            playerVelocity = Vector2.zero;
            _view.SetAnimTrigger("die");
            //�Q�[���I�[�o�[��ʂ̕\��
            AudioManager.Instance.PlaySE(AudioType.gameOver);
            _playerInput.actions.Disable();
            _playerInput.actions.FindActionMap("UI");
            overWindow.SetActive(true);
        }

        //�n�ʂƐڂ��Ă��邩�̔���
        private void IsGround()
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down, 0.01f);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down * 0.01f, Color.red);
            //�n�ʂƐڂ��Ă���Ƃ�
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                currentState |= WizardControlState.Standing;
                return;
            }

            currentState &= ~WizardControlState.Standing;
            return;
        }

        //�W�����v���ȊO�ł�y�����̑��x�̒���
        private void AdjustVelocity_y()
        {
            //�n�ʂƐڂ��Ă���Ƃ���y�����̑��x�����̒l�̂Ƃ��̏���
            if(rb.velocity.y < 0 && currentState.HasFlag(WizardControlState.Standing))
            {
                playerVelocity.y = 0f;
                currentState &= ~WizardControlState.falling;
            }

            //�W�����v���ȊO�ŋ󒆂ɂ���Ƃ��̏���
            if(!currentState.HasFlag(WizardControlState.Jumping) && !currentState.HasFlag(WizardControlState.Standing))
            {
                playerVelocity.y = gravity;
            }
        }
    }
}
