using UnityEngine;

public class SightCheckerExample2D : MonoBehaviour
{
    // 自分自身
    [SerializeField] private Transform _self;

    // ターゲット
    [SerializeField] private Transform _target;

    // 視野角（度数法）
    [SerializeField] private float _sightAngle;

    // 視界の最大距離
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    [SerializeField] private Vector2 direction = Vector2.left;
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public bool IsVisible()
    {
        if (!enemy.GetOnCamera() || enemy.GetHitPoint() <= 0) return false;

        // 自身の位置
        Vector2 selfPos = _self.position;
        // ターゲットの位置
        Vector2 targetPos = _target.position;

        // 自身の向き（正規化されたベクトル）
        // この例では右向きを正面とする
        Vector2 selfDir = direction;

        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector2.Dot(selfDir, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    //directionのyの値が0の時有効
    public void TurnDirection()
    {
        if (direction.y != 0) return;
        direction *= -1;
    }
}