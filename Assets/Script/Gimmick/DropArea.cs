using UnityEngine;
using Cysharp.Threading.Tasks;

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
            player.DamageFromGimmick(10).Forget();
        }

        if (collision.gameObject.TryGetComponent<EnemyPresenter>(out var enemy))
        {
            //�X�R�A�̉��_��Enemy�̔j��
            UIManager.Instance.AddScore(enemy.Model.Score);
            Destroy(enemy);
        }
    }
}
