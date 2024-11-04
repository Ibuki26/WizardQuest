using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Boar : Enemy
{
    [SerializeField] private float adjustValue;
    private RaycastHit2D hit;
    private RaycastHit2D hit2;
    private SightCheckerExample2D SCE;
    [SerializeField]private Vector2 direction = Vector2.left;
    private bool foundPlayer = false;
    private bool stopFlag = false;
    private float foundTime = 0;

    protected override void Start()
    {
        base.Start();
        SCE = GetComponent<SightCheckerExample2D>();
    }

    void Update()
    {
        if (GetHitPoint() <= 0 && transform.localRotation.z > -90)
            transform.Rotate(new Vector3(0, 0, -1));

        if (foundPlayer)
            foundTime += Time.deltaTime;
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
            rb.velocity = GetEnemyVelocity();

            return; 
        }

        //ˆÚ“®æ‚É’n–Ê‚ª‚È‚¢ê‡U‚è•Ô‚Á‚Ä‹tŒü‚«‚ÉˆÚ“®
        if (!CheckGround() || CheckWall(direction))
        {
            var token = this.GetCancellationTokenOnDestroy();
            stopFlag = true;
            SetVelocity_x(0);
            transform.localScale = new Vector3(-transform.localScale.x, 1.7f, 1);
            SetSpeed(-GetSpeed());
            adjustValue *= -1;
            direction = new Vector2(direction.x * -1, 0);
            SCE.TurnDirection();
            await UniTask.Delay(TimeSpan.FromSeconds(0.15f), cancellationToken:token);
            stopFlag = false;
            
            return;
        }

        //Player‚ªŽ‹ŠE‚É“ü‚Á‚½‚Æ‚«‰Á‘¬
        if (SCE.IsVisible() && !foundPlayer)
        {
            AudioManager.Instance.PlaySE(AudioType.find_moster);
            foundPlayer = true;
            if (GetSpeed() > 0)
                SetSpeed(GetSpeed() + 2);
            else if (GetSpeed() < 0)
                SetSpeed(GetSpeed() - 2);
        }

        //Player”­Œ©ó‘Ô‚Ì‰ðœ‚ÆŒ¸‘¬
        if(!SCE.IsVisible() && foundPlayer && foundTime > 2f)
        {
            foundPlayer = false;
            foundTime = 0f;
            if (GetSpeed() > 0)
                SetSpeed(GetSpeed() - 2);
            else if (GetSpeed() < 0)
                SetSpeed(GetSpeed() + 2);
        }

        SetVelocity_x(GetSpeed());

        rb.velocity = GetEnemyVelocity();
    }

    private bool CheckGround()
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x - adjustValue, transform.position.y - 0.6f), Vector2.down, 0.1f);
        Debug.DrawRay(new Vector2(transform.position.x - adjustValue, transform.position.y - 0.6f), Vector2.down * 0.06f, Color.blue);
        if (hit.collider != null && !hit.collider.isTrigger)
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
            if (hit2.collider.TryGetComponent<Enemy>(out var enemy))
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
