using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Cactus : Enemy
{
    [SerializeField] private Needle needle;
    [SerializeField] private Player player;
    [SerializeField] private Sprite[] sprites;
    private SightCheckerExample2D SCE;
    private RaycastHit2D hit;
    private string hitName;
    private bool shootNeedle = false;
    private int shotCount = 0;
    [SerializeField] private bool turn = false;

    protected override void Start()
    {
        base.Start();
        SCE = GetComponent<SightCheckerExample2D>();
    }

    private void Update()
    {
        if (GetHitPoint() <= 0 && transform.localRotation.z > -90)
            transform.Rotate(new Vector3(0, 0, -1));

        if (CheckPlayer())
        {
            if (turn)
            {
                turn = false;
            }
            else
            {
                SCE.TurnDirection();
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        if (SCE.IsVisible() && !shootNeedle)
        {
            ShotNeedle().Forget();
        }
    }

    private async UniTask ShotNeedle()
    {
        var token = this.GetCancellationTokenOnDestroy();
        shotCount += 1;
        if(shotCount > 3)
        {
            shootNeedle = true;
            shotCount = 0;
            sr.sprite = sprites[shotCount];
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken:token);
            shootNeedle = false;
            return;
        }

        shootNeedle = true;
        AudioManager.Instance.PlaySE(AudioType.needle);
        sr.sprite = sprites[shotCount];
        needle.CreateNeedle(player.transform.position, transform.position);
        await UniTask.Delay(TimeSpan.FromSeconds(1f),cancellationToken:token);
        shootNeedle = false;
    }

    //éãñÏÇÃï˚å¸ÇïœçXÇ∑ÇÈä÷êî
    private bool CheckPlayer()
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.52f), Vector2.up, 6f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.52f), Vector2.up * 6f, Color.blue);
        
        if (hit.collider != null && hit.collider.transform.parent != null && hit.collider.transform.parent.TryGetComponent<Player>(out var player))
        {
            if (hitName == hit.collider.name) return false;

            hitName = hit.collider.name;
            return true;
        }

        hitName = null;
        return false;
    }
}
