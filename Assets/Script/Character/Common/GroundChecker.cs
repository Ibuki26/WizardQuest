using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    RaycastHit2D hit;

    //地面と接しているかの判定
    public bool IsGround(float adjustRaycast_y)
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down, 0.01f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down * 0.01f, Color.red);
        //地面と接しているとき
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            return true;
        }

        return false;
    }
}
