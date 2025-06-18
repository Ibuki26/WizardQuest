using UnityEngine;

public class EnemySightChecker : MonoBehaviour
{
    // 自分自身
    [SerializeField] private Transform _self;

    // ターゲット
    [SerializeField] private Transform _target;

    // 視野角（度数法）
    [SerializeField] private float _sightAngle;

    // 視界の最大距離
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public void IsVisible(EnemyPresenter enemy)
    {
        if (!enemy.Model.CurrentState.HasFlag(EnemyControlState.OnCamera) || enemy.Model.HitPoint == 0) return;

        // 自身の位置
        Vector2 selfPos = _self.position;
        // ターゲットの位置
        Vector2 targetPos = _target.position;

        // 自身の向き（正規化されたベクトル）
        // この例では右向きを正面とする
        Vector2 selfDir = new Vector2(enemy.Model.Direction, 0);

        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector2.Dot(selfDir, targetDir.normalized);

        // 視界判定
        if (innerProduct > cosHalf && targetDistance < _maxDistance)
            enemy.Model.CurrentState |= EnemyControlState.FindPlayer;
        else
            enemy.Model.CurrentState &= ~EnemyControlState.FindPlayer;
    }
}
