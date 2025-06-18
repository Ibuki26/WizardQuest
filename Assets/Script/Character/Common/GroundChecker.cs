using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    RaycastHit2D hit;

    //�n�ʂƐڂ��Ă��邩�̔���
    public bool IsGround(float adjustRaycast_y)
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down, 0.01f);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - adjustRaycast_y), Vector2.down * 0.01f, Color.red);
        //�n�ʂƐڂ��Ă���Ƃ�
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            return true;
        }

        return false;
    }
}
