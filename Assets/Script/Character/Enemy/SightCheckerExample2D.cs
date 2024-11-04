using UnityEngine;

public class SightCheckerExample2D : MonoBehaviour
{
    // �������g
    [SerializeField] private Transform _self;

    // �^�[�Q�b�g
    [SerializeField] private Transform _target;

    // ����p�i�x���@�j
    [SerializeField] private float _sightAngle;

    // ���E�̍ő勗��
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    [SerializeField] private Vector2 direction = Vector2.left;
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// �^�[�Q�b�g�������Ă��邩�ǂ���
    /// </summary>
    public bool IsVisible()
    {
        if (!enemy.GetOnCamera() || enemy.GetHitPoint() <= 0) return false;

        // ���g�̈ʒu
        Vector2 selfPos = _self.position;
        // �^�[�Q�b�g�̈ʒu
        Vector2 targetPos = _target.position;

        // ���g�̌����i���K�����ꂽ�x�N�g���j
        // ���̗�ł͉E�����𐳖ʂƂ���
        Vector2 selfDir = direction;

        // �^�[�Q�b�g�܂ł̌����Ƌ����v�Z
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(��/2)���v�Z
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z
        // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        var innerProduct = Vector2.Dot(selfDir, targetDir.normalized);

        // ���E����
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    //direction��y�̒l��0�̎��L��
    public void TurnDirection()
    {
        if (direction.y != 0) return;
        direction *= -1;
    }
}