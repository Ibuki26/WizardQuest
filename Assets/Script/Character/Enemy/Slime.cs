using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Slime : Enemy
{
    [SerializeField] private float adjustValue; //RaycastHit2Dのx座標調整用変数
    private RaycastHit2D hit;
    private RaycastHit2D hit2;
    [SerializeField] Vector2 direction = Vector2.left;
    private float moveTime = 0;
    private bool stopFlag = false;

    private void Update()
    {
        //カメラに映っていて停止状態ではないとき動作時間を計測
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

        //ジャンプしながら移動の処理
        if (moveTime < 0.8f)
        {
            SetVelocity_x(GetSpeed());
            SetVelocity_y(GetJump());
        }
        //落下しながら移動の処理
        else if(moveTime < 1.6f)
        {
            SetVelocity_x(GetSpeed());
            SetVelocity_y(-GetJump());
        }
        //着地したときの処理
        else
        {
            var token = this.GetCancellationTokenOnDestroy();
            stopFlag = true;
            SetVelocity_x(0);
            SetVelocity_y(0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken:token);
            stopFlag = false;
            moveTime = 0f;

            //移動先に地面が無かった場合はspeedを反転させて逆に動くようにする
            if (!CheckGround() || CheckWall(direction))
            {
                SetSpeed(-GetSpeed());
                direction = new Vector2(direction.x * -1, 0);
                adjustValue *= -1;
            }
        }
        
        rb.velocity = GetEnemyVelocity();
    }

    //移動先に地面があるかの判定
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
