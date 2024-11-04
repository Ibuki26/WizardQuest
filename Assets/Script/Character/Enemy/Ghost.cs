using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Ghost : Enemy
{
    [SerializeField] private float turnTime;
    private float moveTime = 0;
    private bool stopFlag = false;

    void Update()
    {
        //ƒJƒƒ‰‚É‰f‚Á‚Ä‚¢‚Ä’â~ó‘Ô‚Å‚Í‚È‚¢‚Æ‚«“®ìŠÔ‚ğŒv‘ª
        if (!stopFlag && GetOnCamera())
            moveTime += Time.deltaTime;

        if (GetHitPoint() <= 0 && transform.localRotation.z > -90)
            transform.Rotate(new Vector3(0, 0, -1));
    }

    private void FixedUpdate()
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

        if (moveTime < turnTime)
        {
            SetVelocity_x(GetSpeed());
            SetVelocity_y(GetJump());
        }
        else if(moveTime >= turnTime)
        {
            var token = this.GetCancellationTokenOnDestroy();
            SetVelocity_x(0);
            SetVelocity_y(0);
            stopFlag = true;
            if (GetSpeed() != 0)
                transform.localScale = new Vector3(-transform.localScale.x, 2, 1);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken:token);
            SetSpeed(-GetSpeed());
            SetJump(-GetJump());
            stopFlag = false;
            moveTime = 0f;
        }

        rb.velocity = GetEnemyVelocity();
    }
}
