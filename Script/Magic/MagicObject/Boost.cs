using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class Boost : MagicObject
{
    private Animator anim;
    private SpriteRenderer sr;
    private AnimatorStateInfo stateInfo;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (anim)
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
    }

    //魔法のクールタイム、攻撃力とプレイヤーのスピードをアップ
    private async UniTask OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out var player))
        {
            AudioManager.Instance.PlaySE(AudioType.boost_up);
            int num;
            Debug.Log(player.magics[0].gameObject.name);
            if (player.magics[0].gameObject.name == "BoostMagic")
                num = 1;
            else
                num = 0;
            transform.parent = player.gameObject.transform;

            var token = this.GetCancellationTokenOnDestroy();
            player.magics[num].SetCoolTime(player.magics[num].GetCoolTime() - 0.2f);
            player.magics[num].magicObject.SetAttack(player.magics[num].magicObject.GetAttack() + 5);
            player.SetSpeed(player.GetSpeed() + 2);
            await UniTask.WaitUntil(() => stateInfo.IsName("Hit-1 Animation"));
            await UniTask.WaitUntil(() => stateInfo.normalizedTime >= 1f);
            sr.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: token);
            AudioManager.Instance.PlaySE(AudioType.boost_down);
            player.magics[num].SetCoolTime(player.magics[num].GetCoolTime() + 0.2f);
            player.magics[num].magicObject.SetAttack(player.magics[num].magicObject.GetAttack() - 5);
            player.SetSpeed(player.GetSpeed() - 2);
            sr.enabled = true;
            anim.SetBool("disappear", true);
             await UniTask.WaitUntil(() => stateInfo.IsName("Hit-3 Animation"));
            await UniTask.WaitUntil(() => stateInfo.normalizedTime >= 1f);
            Destroy(gameObject);
        }
    }

    public override void Effect(Enemy enemy)
    {
        return;
    }
}
