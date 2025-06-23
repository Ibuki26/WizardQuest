using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;
using ShotMagicMethod;

public abstract class EnemyPresenter : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int strength;
    [SerializeField] private int defense;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int score;
    [SerializeField] private int direction;

    private EnemyModel _model;
    private EnemyView _view;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Tween tween { get; set; }
    private Vector2 enemyVelocity = Vector2.zero;

    public EnemyModel Model => _model;
    public EnemyView View => _view;
    public Rigidbody2D RB => rb;
    public Tween Tween
    {
        get { return tween; }
        set { tween = value; }
    }

    ////InGame��Start�ŌĂ΂��֐� ���̊֐��̎����I��Start
    public virtual void ManualStart()
    {
        _model = new EnemyModel(hp, strength, defense, xSpeed, ySpeed, moveSpeed, score, direction);
        _view = GetComponent<EnemyView>();
        _view.ManualStart();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        //�����̒���
        transform.localScale = new Vector3(transform.localScale.x * _model.Direction, transform.localScale.y, transform.localScale.z);
    }

    //InGame��FixedUpdate�ŌĂ΂��֐� ���̊֐��̎����I��FixedUpdate
    public abstract void ManualFixedUpdate();

    private void OnTriggerStay2D(Collider2D collision)
    {
        //�v���C���[�̖��@�ɓ��������Ƃ��̏����@�_���[�W������ԂȂ�s��Ȃ�
        if (collision.TryGetComponent<ShotMagic>(out var shotMagic)
            && !_model.CurrentState.HasFlag(EnemyControlState.IgnoreDamage))
        {
            Damage(shotMagic.Status.Attack, shotMagic.Model.Strength).Forget();
            //shoot�^Magic�̓��������Ƃ��̌���
            var method = shotMagic as IShotMagicEffect;
            if (method != null)
                method.Effect(_model);
            //�X�L���̎��s
            var context = new MagicHitContext(shotMagic.Model, shotMagic.Status, _model, 0);
            SkillManager.Instance.TriggerOnMagicHit(context);
        }

        //AreaMagic���������̂��ߖ�����
        if (collision.TryGetComponent<AreaMagic>(out var areaMagic)
            && !_model.CurrentState.HasFlag(EnemyControlState.IgnoreDamage))
        {

        }

        //�v���C���[�Ƀ_���[�W��^���鏈��
        if (collision.transform.parent != null
            && collision.transform.parent.TryGetComponent<WizardPresenter>(out var player))
        {
            //Attack�̐��l���_���[�W�O�ɑ������
            player.DamageFromEnemy(_model.Attack, _model.Strength).Forget();
        }
    }

    private void OnBecameVisible()
    {
        _model.CurrentState |= EnemyControlState.OnCamera;
    }

    private void OnBecameInvisible()
    {
        _model.CurrentState &= ~EnemyControlState.OnCamera;
    }

    //Enemy���_���[�W���󂯂鏈��
    private async UniTask Damage(int playerAttack, int playerStrength)
    {
        if (_model.HitPoint < 0) return;

        //���G��Ԃ̊J�n
        _model.CurrentState |= EnemyControlState.IgnoreDamage;
        //�_���[�W���o
        AudioManager.Instance.PlaySE(AudioType.damage_moster);
        _view.DamageColor().Forget();
        //�_���[�W�ʂ̌v�Z
        var resultDamage = playerAttack + playerStrength / 5 - _model.Defense / 10;
        //�̗�-�_���[�W�ʂ����̒l�ɂȂ�����0�A�����łȂ��Ȃ�̗�-�_���[�W�ʂ����̂܂ܑ��
        _model.HitPoint = (_model.HitPoint - resultDamage < 0)
            ? 0 : _model.HitPoint - resultDamage;

        Die().Forget();

        Debug.Log("attack : " + playerAttack + ", strength : " + playerStrength + ", resulr : " + resultDamage);

        //���G��Ԃ̑ҋ@
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
        //���G��Ԃ̉���
        _model.CurrentState &= ~EnemyControlState.IgnoreDamage;
    }

    //Enemy���|���ꂽ�Ƃ��̏���
    public async UniTask Die()
    {
        if (_model.HitPoint > 0) return;

        //�X�R�A�̉��Z
        UIManager.Instance.AddScore(_model.Score);
        //�ړ���Ԃ̉���
        _model.CurrentState &= ~EnemyControlState.Moving;

        //���ݎ��s���̃A�j���[�V�������~
        tween.Kill();
        AudioManager.Instance.PlaySE(AudioType.die_moster);
        coll.enabled = false;
        //x�����̓������~�߁A���ɗ��Ƃ�
        enemyVelocity = new Vector2(0, -4.0f);
        rb.velocity = enemyVelocity;
        //��]�̃A�j���[�V����
        tween = transform.DORotate(new Vector3(0, 0, -90 * _model.Direction), 1);
        //�X�R�A�̒ǉ�
        //�J�����ɉf��Ȃ��Ȃ�����A�A�j���[�V�������I���A�j��
        await UniTask.WaitUntil(() => !_model.CurrentState.HasFlag(EnemyControlState.OnCamera));
        tween.Kill();
        Destroy(gameObject);
    }

    //Enemy����莞�Ԏ~�߂�֐�
    public async UniTask Stop(float stopTime)
    {
        if (_model.CurrentState.HasFlag(EnemyControlState.Stopped)) return;
        if (_model.HitPoint == 0) return;

        //��~��Ԃ̊J�n
        _model.CurrentState |= EnemyControlState.Stopped;
        _model.CurrentState &= ~EnemyControlState.Moving;
        StopOrder(stopTime);
        //��~��Ԃ̑ҋ@
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.Delay(TimeSpan.FromSeconds(stopTime), cancellationToken: token);
        //��~��Ԃ̉���
        _model.CurrentState &= ~EnemyControlState.Stopped;
    }

    //Enemy���m�b�N�o�b�N����֐�
    public async UniTask KnockBack(float x)
    {
        if (_model.HitPoint == 0) return;

        //���������o���Ȃ��悤�ɒ�~��Ԃɂ���
        _model.CurrentState |= EnemyControlState.Stopped;
        //���O�ɂ���Ă���DoTween�A�j���[�V�����̔j��
        tween.Kill();
        tween = transform.DOMoveX(transform.position.x + x, 0.2f, false);
        //�����҂��čs���̍ĊJ
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: this.GetCancellationTokenOnDestroy());
        //x�����ړ��̃A�j���[�V������j��
        //tween.Kill();
        _model.CurrentState &= ~EnemyControlState.Moving;
        _model.CurrentState &= ~EnemyControlState.Stopped;
    }

    public abstract void StopOrder(float stopTime);
}
