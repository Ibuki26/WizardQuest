using UnityEngine;

public class BandS 
{
    public BandS(bool flag, string name)
    {
        this.flag = flag;
        this.name = name;
    }

    public  bool flag;
    public string name;
}


public static class RaycastHelper
{
    //壁や床があるか確認する関数
    public static BandS CheckGroundAndWallsBS(Vector2 origin, Vector2 direction, float distance, Color color)
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
                return new BandS(false,hit.collider.name);
            }

            return new BandS(true,hit.collider.name);
        }

        return new BandS(false,"none");
    }

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
