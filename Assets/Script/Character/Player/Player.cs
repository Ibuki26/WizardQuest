using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private int hitPoint;
    [SerializeField] private float speed; //x�������̑���
    [SerializeField] private float jump; //y�������̑���
    [SerializeField] private float jumpLimitTime;
    [SerializeField] private float gravity; //������Ƃ��̑���
    [SerializeField] private float adjustValue;
    [SerializeField] private UIManager ui;
    [SerializeField] private MagicButtonText mbText;
    public Magic[] magics = new Magic[2];

    private Rigidbody2D rb;
    private RaycastHit2D hit;
    private Animator anim;
    private PlayerInput playerInput;
    private Vector2 playerVelocity = Vector2.zero; //���x�ɑ������p�̕ϐ�
    private AnimatorStateInfo stateInfo;
    private Wand wand;
    private float jumpTime = 0;
    private int maxHitPoint;
    private bool isStanding = false; //�n�ʂƐڂ��Ă��邩���L�^����ϐ�
    private bool isJumping = false; //�W�����v�������L�^����ϐ�
    private bool isMagicing = false;
    private bool ignoreDamage = false;

    void Start()
    {
        Debug.Log("start");
        //���@�N���X���쐬���āA�e�I�u�W�F�N�g��Player�ɂ���
        GameObject magic1 = Instantiate(MySetedMagic.Instance.GetMagic(0), transform.position, Quaternion.identity);
        magic1.transform.parent = transform;
        GameObject magic2 = Instantiate(MySetedMagic.Instance.GetMagic(1), transform.position, Quaternion.identity);
        magic2.transform.parent = transform;
        //Player��magics�ɍ쐬����magic1,2��o�^
        magics[0] = magic1.GetComponent<Magic>();
        magics[1] = magic2.GetComponent<Magic>();
        //��̓o�^
        wand = MySetedMagic.Instance.GetWand();
        wand.Effect(this);
        ui.SetMagic(magics[0], magics[1]);
        mbText.SetMagic(magics[0], magics[1]);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        maxHitPoint = hitPoint;
    }

    private void OnDestroy()
    {
        wand.Reset(this);
    }

    // UIManager�Ŏg��Get�֐�
    public int GetHitPoint()
    {
        return hitPoint;
    }

    public int GetMaxHitPoint()
    {
        return maxHitPoint;
    }

    
    public Magic GetMagic(int index)
{
    if (index >= 0 && index < magics.Length)
    {
        return magics[index];
    }
    return null;
}
    

    void Update()
    {
        if (anim)
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        //�W�����v���Ԃ̌v��
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        isStanding = IsGround();

        //�W�����v���Ԃ��؂ꂽ���̏���
        if(jumpTime > jumpLimitTime)
        {
            anim.SetBool("isJump", false);
            playerVelocity.y = -gravity;
            isJumping = false;
            jumpTime = 0f;
        }

        if(hitPoint > 0)
            rb.velocity = playerVelocity;
    }

    //x�������ړ��p���\�b�h
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            anim.SetBool("isRun", false);
            playerVelocity.x = 0f;
        }
        
        if (!context.performed) return;

        //�����̓��͏����擾
        float xAxis = context.ReadValue<Vector2>().x;

        if (xAxis > 0)
        {
            transform.localScale = new Vector3(0.35f, 0.35f, 1);
            //���@��x���W���x�Ɣ������W�̕ύX
            magics[0].magicObject.SetSpeed(Mathf.Abs(magics[0].magicObject.GetSpeed()));
            magics[0].SetAdjustPos_x(Mathf.Abs(magics[0].GetAdjustPos_x()));
            magics[0].SetMagic(1);
            magics[1].magicObject.SetSpeed(Mathf.Abs(magics[1].magicObject.GetSpeed()));
            magics[1].SetAdjustPos_x(Mathf.Abs(magics[1].GetAdjustPos_x()));
            magics[1].SetMagic(1);
        }
        else if (xAxis < 0)
        {
            transform.localScale = new Vector3(-0.35f, 0.35f, 1);
            //���@��x���W���x�Ɣ������W�̕ύX
            magics[0].magicObject.SetSpeed(-Mathf.Abs(magics[0].magicObject.GetSpeed()));
            magics[0].SetAdjustPos_x(-Mathf.Abs(magics[0].GetAdjustPos_x()));
            magics[0].SetMagic(-1);
            magics[1].magicObject.SetSpeed(-Mathf.Abs(magics[1].magicObject.GetSpeed()));
            magics[1].SetAdjustPos_x(-Mathf.Abs(magics[1].GetAdjustPos_x()));
            magics[1].SetMagic(-1);
        }

        anim.SetBool("isRun", true);
        playerVelocity.x = xAxis * speed;
    }

    //�W�����v�̏���
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isStanding)
        {
            AudioManager.Instance.PlaySE(AudioType.jump);
            anim.SetBool("isJump", true);
            playerVelocity.y = jump;
            isJumping = true;
        }
    }

    //�{�^���𗣂��ăW�����v���~�߂��Ƃ��̏���
    public void OnStopJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        anim.SetBool("isJump", false);
        playerVelocity.y = -gravity;
        isJumping = false;
        jumpTime = 0f;
    }

    public async void OnMagic1(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isMagicing || magics[0].IsCooling()) return;

        isMagicing = true;
        anim.SetTrigger("attack");
        await UniTask.WaitUntil(() => stateInfo.IsName("Attack"));
        await UniTask.WaitUntil(() => 0.9f <= stateInfo.normalizedTime);
        magics[0].Activation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        isMagicing = false;
    }

    public async void OnMagic2(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isMagicing || magics[1].IsCooling()) return;

        isMagicing = true;
        anim.SetTrigger("attack");
        await UniTask.WaitUntil(() => stateInfo.IsName("Attack"));
        await UniTask.WaitUntil(() => 0.9f <= stateInfo.normalizedTime);
        magics[1].Activation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        isMagicing = false;
    }

    public void OnOpenWindow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        AudioManager.Instance.PlaySE(AudioType.openWindow);
        playerInput.SwitchCurrentActionMap("UI");
        ui.ActiveWindow("option");
        ui.ActiveWindow("mouse");
    }

    public async void Damage(int damage)
    {
        if (damage < 0) return;
        if (ignoreDamage) return;
        if (stateInfo.IsName("Die")) return;

        ignoreDamage = true;

        //HitPoint�̃_���[�W�̌v�Z����
        int newHitPoint = hitPoint - damage;
        if (newHitPoint < 0)
            hitPoint = 0;
        else
            hitPoint -= damage;

        //�̗͂�0�ȉ��ɂȂ�����Q�[���I�[�o�[
        if (hitPoint <= 0)
        {
            AudioManager.Instance.PlaySE(AudioType.gameOver);
            anim.SetTrigger("die");
            playerInput.SwitchCurrentActionMap("UI");
            ui.ActiveWindow("over");
            ui.ActiveWindow("mouse");

            return;
        }

        AudioManager.Instance.PlaySE(AudioType.damage_player);
        //�L�����̓_��
        anim.SetTrigger("hurt");
     
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        ignoreDamage = false;
    }

    public void Heal(int healPoint)
    {
        if (hitPoint + healPoint > maxHitPoint)
            hitPoint = maxHitPoint;
        else
            hitPoint += healPoint;
    }

    //�n�ʂƐڂ��Ă��邩�̔���
    private bool IsGround()
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustValue), Vector2.down, 0.06f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustValue), Vector2.down * 0.06f, Color.red);
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            if (!isJumping)
                playerVelocity.y = 0f;

            return true;
        }

        if (!isJumping)
            playerVelocity.y = -gravity;

        return false;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetHP()
    {
        return hitPoint;
    }

    public void SetSpeed(float f)
    {
        speed = f;
    }

    public void SetHP(int hp)
    {
        if (hp < 0) return;

        hitPoint = hp;
    }
}
