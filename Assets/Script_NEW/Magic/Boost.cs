using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardPlayer;
using Cysharp.Threading.Tasks;
using System;

namespace WizardMagic
{
    public class Boost : BuffMagic
    {
        private Animator anim;
        private SpriteRenderer sr;

        public async override UniTask Buff(WizardPresenter player, float destroyTime)
        {
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();

            //Player��Status����
            player.Model.Strength += 25;
            player.Model.Defense += 30;
            player.Model.Speed += 30;

            //�A�j���[�V�������I���܂őҋ@
            Hidden().Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(destroyTime),
                cancellationToken: this.GetCancellationTokenOnDestroy());
            Deactivate(player);
        }

        public override void Deactivate(WizardPresenter player)
        {
            sr.enabled = true;
            AudioManager.Instance.PlaySE(AudioType.boost_down);
            
            //Player��Status�����̉���
            player.Model.Strength -= 25;
            player.Model.Defense -= 30;
            player.Model.Speed -= 30;

            anim.SetBool("disappear", true);
            //�A�j���[�V�������I���܂őҋ@
            WaitDestroy().Forget();
        }

        private async UniTask Hidden()
        {
            await UniTask.WaitUntil(() =>
               anim.GetCurrentAnimatorStateInfo(0).IsName("Hit-1 Animation")
               && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
               , cancellationToken: this.GetCancellationTokenOnDestroy());

            sr.enabled = false;
        }

        private async UniTask WaitDestroy()
        {
            await UniTask.WaitUntil(() =>
                anim.GetCurrentAnimatorStateInfo(0).IsName("Hit-3 Animation")
                && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f
                , cancellationToken: this.GetCancellationTokenOnDestroy());

            Destroy(gameObject);
        }
    }
}
