using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Slime : Enemy
{
    [SerializeField] private float adjustValue; //RaycastHit2D��x���W�����p�ϐ�
    private RaycastHit2D hit;
    private RaycastHit2D hit2;
    [SerializeField] Vector2 direction = Vector2.left;
    private float moveTime = 0;
    private bool stopFlag = false;

    private void Update()
    {
        //�J�����ɉf���Ă��Ē�~��Ԃł͂Ȃ��Ƃ����쎞�Ԃ��v��
        if(!stopFlag && GetOnCamera())
            moveTime += Time.deltaTime;

        if (GetHitPoint() <= 0 && transform.localRotation.z > -90)
            transform.Rotate(new Vector3(0, 0, -1));
    }

    void FixedUpdate()
    {
        Move().Forget();
    }

    private async UniTask Move()
    {
        if (GetHitPoint() <= 0 || GetStoped()) return;
        if (stopFlag || !GetOnCamera())
        {
            SetVelocity_x(0);
            SetVelocity_y(0);
            rb.velocity = GetEnemyVelocity();

            return;
        }

        //�W�����v���Ȃ���ړ��̏���
        if (moveTime < 0.8f)
        {
            SetVelocity_x(GetSpeed());
            SetVelocity_y(GetJump());
        }
        //�������Ȃ���ړ��̏���
        else if(moveTime < 1.6f)
        {
            SetVelocity_x(GetSpeed());
            SetVelocity_y(-GetJump());
        }
        //���n�����Ƃ��̏���
        else
        {
            var token = this.GetCancellationTokenOnDestroy();
            stopFlag = true;
            SetVelocity_x(0);
            SetVelocity_y(0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken:token);
            stopFlag = false;
            moveTime = 0f;

            //�ړ���ɒn�ʂ����������ꍇ��speed�𔽓]�����ċt�ɓ����悤�ɂ���
            if (!CheckGround() || CheckWall(direction))
            {
                SetSpeed(-GetSpeed());
                direction = new Vector2(direction.x * -1, 0);
                adjustValue *= -1;
            }
        }
        
        rb.velocity = GetEnemyVelocity();
    }

    //�ړ���ɒn�ʂ����邩�̔���
    private bool CheckGround()
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x-adjustValue, transform.position.y-0.4f), Vector2.down, 0.1f);
        Debug.DrawRay(new Vector2(transform.position.x-adjustValue, transform.position.y-0.4f), Vector2.down * 0.1f, Color.blue);
        if(hit.collider != null && !hit.collider.isTrigger)
        {
            return true;
        }

        return false;
    }

    private bool CheckWall(Vector2 dir)
    {
        hit2 = Physics2D.Raycast(new Vector2(transform.position.x - adjustValue, transform.position.y), dir, 0.1f);
        Debug.DrawRay(new Vector2(transform.position.x - adjustValue, transform.position.y), dir * 0.1f, Color.red);
        if (hit2.collider != null && !hit2.collider.isTrigger)
        {
            if(hit2.collider.TryGetComponent<Enemy>(out var enemy))
            {
                return false;
            }

            if (hit2.collider.TryGetComponent<Player>(out var player))
            {
                return false;
            }

            return true;
        }

        return false;
    }
}
