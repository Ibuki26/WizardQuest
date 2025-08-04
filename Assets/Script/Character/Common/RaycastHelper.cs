using UnityEngine;

public static class RaycastHelper
{
    //壁や床があるか確認する関数
    public static bool CheckGroundAndWalls(Vector2 origin, Vector2 direction, float distance, Color color)
    {
        var hit = Physics2D.Raycast(origin, direction, distance);
        Debug.DrawRay(origin, direction * distance, color);
        //他オブジェクトにぶつかったとき
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            //ぶつかったオブジェクトが他EnemyかPlayerのときは無視
            if (hit.collider.TryGetComponent<EnemyPresenter>(out _) ||
                hit.collider.TryGetComponent<WizardPresenter>(out _))
            {
                return false;
            }

            return true;
        }

        return false;
    }
}
