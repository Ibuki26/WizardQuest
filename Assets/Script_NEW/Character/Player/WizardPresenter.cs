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
        private CancellationTokenSource jump_cts; //OnJumpで使用するawaitのキャンセル用トークン

        private float gravity = -4; //落下時のy方向の速度
        private float adjustRaycast_y = 0.005f; //Raycastをするときの発生座標の調整用数値
        private float jumpLimit = 0.7f; //ジャンプする限界時間
        private float standardSpeed = 4.0f; //x方向の移動速度の基準値
        private Vector2 playerVelocity = Vector2.zero; //プレイヤーの移動速度を記録する変数

        public WizardModel Model => _model;
        public MagicCreator[] Magics => magics;

        private void SetM()
        {
            //魔法のセット
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

            //Magicの取得
            SetM();
            //装備の取得
            //UIへの情報の送信　いるか不明
        }

        public void ManualFixedUpdate()
        {
            //地面の接地判定
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
            if (_model.HitPoint.Value == 0) return; //体力が0のときは動かない

            //スティックに触っていないときの処理
            if (context.canceled)
            {
                _view.SetAnimation("isRun", false);
                playerVelocity.x = 0f;
                return;
            }

            //x軸方向の入力情報の取得
            float xAxis = context.ReadValue<Vector2>().x;

            if (xAxis == 0) return;
            //進行方向の記録と画像の向き調整
            _model.Direction = (xAxis > 0) ? 1 : -1;
            transform.localScale = new Vector3(0.35f * _model.Direction, 0.35f, 1);

            _view.SetAnimation("isRun", true);
            playerVelocity.x = standardSpeed * _model.Speed/100 * _model.Direction;
        }

        public async void OnJump(InputAction.CallbackContext context)
        {
            if (context.action.name != "Jump") return;
            if (_model.HitPoint.Value == 0) return; //体力が0のときは動かない

            jump_cts?.Cancel();
            jump_cts = new CancellationTokenSource();

            if (context.canceled && currentState.HasFlag(WizardControlState.Jumping)){
                //下のif文のDelayをキャンセルして落ちる処理に移行させる
                jump_cts?.Cancel();

                return;
            }
            
            if (!context.performed) return;

            if (currentState.HasFlag(WizardControlState.Standing)){
                //ジャンプを始める処理
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
                    //落ちる処理
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
            //魔法実行中とクールタイム中のときは実行しない
            if (currentState.HasFlag(WizardControlState.Magicing) || magics[0].IsCoolTime) return;
            if (_model.HitPoint.Value == 0) return; //体力が0のときは動かない

            currentState |= WizardControlState.Magicing;
            _view.SetAnimTrigger("attack");
            　
            //アニメーションのタイミング調整
            await UniTask.WaitUntil(() =>
                _view.StateInfo.IsName("Attack") &&
                _view.StateInfo.normalizedTime >= 0.9f
            , cancellationToken : this.GetCancellationTokenOnDestroy());

            magics[0].CreateMagic(this, 0);
            //別のボタンの魔法をほぼ同時に使えないように待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            currentState &= ~WizardControlState.Magicing;
        }

        private async void OnMagic2(InputAction.CallbackContext context)
        {
            if (context.action.name != "Magic2") return;
            if (!context.performed) return;
            //魔法実行中とクールタイム中のときは実行しない
            if (currentState.HasFlag(WizardControlState.Magicing) || magics[1].IsCoolTime) return;
            if (_model.HitPoint.Value == 0) return; //体力が0のときは動かない

            currentState |= WizardControlState.Magicing;
            _view.SetAnimTrigger("attack");

            //アニメーションのタイミング調整
            await UniTask.WaitUntil(() =>
                _view.StateInfo.IsName("Attack") &&
                _view.StateInfo.normalizedTime >= 0.9f
            , cancellationToken : this.GetCancellationTokenOnDestroy());

            magics[1].CreateMagic(this, 1);
            //別のボタンの魔法をほぼ同時に使えないように待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            currentState &= ~WizardControlState.Magicing;
        }

        public async UniTask Damage(int enemyAttack, int enemyStrength)
        {
            if (currentState.HasFlag(WizardControlState.IgnoreDamage)
                || currentState.HasFlag(WizardControlState.Magicing)) return;
            if (_model.HitPoint.Value == 0) return;

            //無敵状態の開始
            currentState |= WizardControlState.IgnoreDamage;
           
            //ダメージ量の計算
            var resultDamage = enemyAttack + enemyStrength / 5 - _model.Defense/10;
            //体力-ダメージ量が負の値になったら0、そうでないなら体力-ダメージ量をそのまま代入
            _model.HitPoint.Value = (_model.HitPoint.Value - resultDamage < 0) ? 0 : _model.HitPoint.Value - resultDamage;
            Die();
            
            //ダメージ演出
            AudioManager.Instance.PlaySE(AudioType.damage_player);
            _view.SetAnimTrigger("hurt");
            
            //無敵状態の待機
            var token = this.GetCancellationTokenOnDestroy();
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            //無敵状態の解除
            currentState &= ~WizardControlState.IgnoreDamage;
        }

        public void Heal(int healPoint)
        {
            //今の体力と回復値を足して、上限値を超えたら上限値を代入、そうでないなら体力+回復値を代入
            _model.HitPoint.Value = (_model.HitPoint.Value + healPoint > _model.MaxHitPoint) ? _model.MaxHitPoint : _model.HitPoint.Value + healPoint;
        }

        private void Die()
        {
            if (_model.HitPoint.Value > 0) return;

            AudioManager.Instance.PlaySE(AudioType.gameOver);
            playerVelocity = Vector2.zero;
            _view.SetAnimTrigger("die");
            //ゲームオーバー画面の表示
            AudioManager.Instance.PlaySE(AudioType.gameOver);
            _playerInput.actions.Disable();
            _playerInput.actions.FindActionMap("UI");
            overWindow.SetActive(true);
        }

        //地面と接しているかの判定
        private void IsGround()
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down, 0.01f);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down * 0.01f, Color.red);
            //地面と接しているとき
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                currentState |= WizardControlState.Standing;
                return;
            }

            currentState &= ~WizardControlState.Standing;
            return;
        }

        //ジャンプ中以外でのy方向の速度の調整
        private void AdjustVelocity_y()
        {
            //地面と接しているときにy方向の速度が負の値のときの処理
            if(rb.velocity.y < 0 && currentState.HasFlag(WizardControlState.Standing))
            {
                playerVelocity.y = 0f;
                currentState &= ~WizardControlState.falling;
            }

            //ジャンプ中以外で空中にいるときの処理
            if(!currentState.HasFlag(WizardControlState.Jumping) && !currentState.HasFlag(WizardControlState.Standing))
            {
                playerVelocity.y = gravity;
            }
        }
    }
}
