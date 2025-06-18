using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    RaycastHit2D hit;

    //’n–Ê‚ÆÚ‚µ‚Ä‚¢‚é‚©‚Ì”»’è
    public bool IsGround(float adjustRaycast_y)
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down, 0.01f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down * 0.01f, Color.red);
        //’n–Ê‚ÆÚ‚µ‚Ä‚¢‚é‚Æ‚«
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            return true;
        }

        return false;
    }
}
