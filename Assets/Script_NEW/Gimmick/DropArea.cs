using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using WizardPlayer;
using WizardEnemy;

namespace WizardGimmick
{
    public class DropArea : MonoBehaviour
    {
        [SerializeField] private Vector3 pos;
        [SerializeField] private MainCamera mainCamera;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<WizardPresenter>(out var player))
            {
                //�v���C���[���w��̈ʒu�ɖ߂��A�_���[�W��^����
                //�J�����̈ʒu���ړ�����
                player.transform.position = pos;
                mainCamera.Move(new Vector3(pos.x, 0, -10));
                AudioManager.Instance.PlaySE(AudioType.damage_player);
                player.Damage(10, 0).Forget();
            }

            if (collision.gameObject.TryGetComponent<EnemyPresenter>(out var enemy))
            {
                //�X�R�A�̉��_��Enemy�̔j��
                WizardUI.UIManager.Instance.AddScore(enemy.Model.Score);
                Destroy(enemy);
            }
        }
    }
}
