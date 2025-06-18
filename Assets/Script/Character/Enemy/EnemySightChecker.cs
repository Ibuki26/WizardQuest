using UnityEngine;

public class EnemySightChecker : MonoBehaviour
{
    // �������g
    [SerializeField] private Transform _self;

    // �^�[�Q�b�g
    [SerializeField] private Transform _target;

    // ����p�i�x���@�j
    [SerializeField] private float _sightAngle;

    // ���E�̍ő勗��
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    /// <summary>
    /// �^�[�Q�b�g�������Ă��邩�ǂ���
    /// </summary>
    public void IsVisible(EnemyPresenter enemy)
    {
        if (!enemy.Model.CurrentState.HasFlag(EnemyControlState.OnCamera) || enemy.Model.HitPoint == 0) return;

        // ���g�̈ʒu
        Vector2 selfPos = _self.position;
        // �^�[�Q�b�g�̈ʒu
        Vector2 targetPos = _target.position;

        // ���g�̌����i���K�����ꂽ�x�N�g���j
        // ���̗�ł͉E�����𐳖ʂƂ���
        Vector2 selfDir = new Vector2(enemy.Model.Direction, 0);

        // �^�[�Q�b�g�܂ł̌����Ƌ����v�Z
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(��/2)���v�Z
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z
        // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        var innerProduct = Vector2.Dot(selfDir, targetDir.normalized);

        // ���E����
        if (innerProduct > cosHalf && targetDistance < _maxDistance)
            enemy.Model.CurrentState |= EnemyControlState.FindPlayer;
        else
            enemy.Model.CurrentState &= ~EnemyControlState.FindPlayer;
    }
}
