using UnityEngine;
using UnityEditor;

public class EnemySightChecker : MonoBehaviour
{
    [SerializeField] private Transform target; //プレイヤーのTransformを保存
    [SerializeField] private float viewFov; //視野範囲の角度
    [SerializeField] private float viewDistance; //視界の最大距離

    public bool ScanForPlayer(int direction)
    {
        //ターゲットへの方向ベクトル
        Vector3 dir = target.position - transform.position;

        //ターゲットの位置が視界より外側にいるか確認、いたら終了
        if (dir.sqrMagnitude > viewDistance * viewDistance) 
            return false;

        Vector2 forwardDirection = new Vector2(direction, 0);

        //キャラクターの正面と方向ベクトルのなす角の角度
        float angle = Vector3.Angle(forwardDirection, dir);

        //ターゲットが視野の外側にいるか確認、いたら終了
        if (angle > viewFov * 0.5f)
            return false;

        return true;
    }
    
    /*
    //シーン上で視界の表示
    private void OnDrawGizmosSelected()
    {
        int direction;

        if (Application.isPlaying)
            direction = GetComponent<EnemyPresenter>().Model.Direction;
        else
            direction = GetComponent<EnemyPresenter>().direction;

        Vector3 startPoint = Quaternion.Euler(0, 0, viewFov * 0.5f) * new Vector2(direction, 0);

        Handles.color = new Color(1.0f, 0, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, startPoint, viewFov, viewDistance);
        Handles.color = Color.white;
    }
    */
}