using UnityEngine;
using UnityEditor;

public class CharacterRaycaster : MonoBehaviour
{
    //天井を確認する場合はインスペクターでtrueにする
    [SerializeField] private bool onCeiling = false;

    public LayerMask groundedLayerMask;
    public float raycastDistance; //天井や床へのRaycastするときの線の長さ
    public float raycastDistanceWall; //壁へのRaycastするときの線の長さ
    public float groundedRaycastAdjust_y; //地面へのRaycastのy座標を調整する数値
    public float nextGroundedRaycastAdjust_x; //移動先の地面へのRaycastのx座標を調整する数値

    private CapsuleCollider2D capsule; //プレイヤーの当たり判定
    private BoxCollider2D box; //敵キャラクターの当たり判定
    private ContactFilter2D contactFilter;
    private RaycastHit2D[] hitBuffers = new RaycastHit2D[5]; //Raycastの情報を記録する配列
    Vector2[] raycastPositions = new Vector2[3]; //Raycastの始点。左、中央、右で3つ

    private bool isGrounded = false; //地面との接触状態を記録
    private bool isCeilinged = false; //天井との接触状態を記録
    private bool isWalled = false; //壁との接触状態を記録
    private bool isNextGrounded = false; //移動先の地面があるかを記録

    public void ManualStart()
    {
        capsule = GetComponent<CapsuleCollider2D>();
        box = GetComponent<BoxCollider2D>();

        //Raycastのフィルタを設定
        contactFilter.layerMask = groundedLayerMask;
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = false;
    }

    public void ManualFixedUpdate()
    {
        isGrounded = CheckGroundCollision();
        isCeilinged = CheckCeilingCollision();
    }

    //地面の判定を確認する関数
    private bool CheckGroundCollision()
    {
        Vector2 raycastDirection;
        Vector2 raycastStart;

        //Raycastの始点を計算
        raycastStart = (Vector2)transform.position + new Vector2(0, groundedRaycastAdjust_y);

        //Raycastの方向を計算
        raycastDirection = Vector2.down;
        Vector2 size = Vector2.zero;
        
        //当たり判定のサイズを取得
        if (capsule != null)
            size = capsule.size;
        else if (box != null)
            size = box.size;

        Vector3 colliderSize = size * transform.lossyScale;

        raycastPositions[0] = raycastStart + Vector2.left * colliderSize.x * 0.4f;
        raycastPositions[1] = raycastStart;
        raycastPositions[2] = raycastStart + Vector2.right * colliderSize.x * 0.4f;

        //地面があるか判定
        for(int i = 0; i < raycastPositions.Length; i++)
        {
            int count = Physics2D.Raycast(raycastPositions[i], raycastDirection, contactFilter, hitBuffers, raycastDistance);

            //キャラクターの中心地を計算
            float offset = 0;
            //当たり判定のオフセットを取得
            if (capsule != null)
                offset = capsule.offset.y;
            else if (box != null)
                offset = box.offset.y;
            float characterBottomHeight = transform.position.y + offset * transform.lossyScale.y;

            //3本の内どれか一つでもヒットし、座標が下ならtrue
            if (count != 0 && characterBottomHeight > hitBuffers[0].point.y)
            {
                //hitBuffersの初期化
                for(int j = 0; j < hitBuffers.Length; j++)
                {
                    hitBuffers[j] = new RaycastHit2D();
                }

                return true;
            }
        }

        //hitBuffersの初期化
        for (int j = 0; j < hitBuffers.Length; j++)
        {
            hitBuffers[j] = new RaycastHit2D();
        }

        return false;
    }

    //天井の判定を確認する関数
    private bool CheckCeilingCollision()
    {
        if (!onCeiling) return false;

        Vector2 raycastDirection;
        Vector2 raycastStart = Vector2.zero;

        //Raycastの始点を計算
        float offset = 0;
        float sizeY = 0;
        if (capsule != null)
        {
            offset = capsule.offset.y;
            sizeY = capsule.size.y;
        }
        else if (box != null)
        {
            offset = box.offset.y;
            sizeY = box.size.y;
        }
        raycastStart = (Vector2)transform.position + new Vector2(0, (offset + sizeY * 0.5f) * transform.lossyScale.y);

        //Raycastの方向を計算
        raycastDirection = Vector2.up;
        Vector2 size = Vector2.zero;

        //当たり判定のサイズを取得
        if (capsule != null)
            size = capsule.size;
        else if (box != null)
            size = box.size;
        Vector3 colliderSize = size * transform.lossyScale;

        raycastPositions[0] = raycastStart + Vector2.left * colliderSize.x * 0.4f;
        raycastPositions[1] = raycastStart;
        raycastPositions[2] = raycastStart + Vector2.right * colliderSize.x * 0.4f;

        for(int i = 0; i < raycastPositions.Length; i++)
        {
            int count = Physics2D.Raycast(raycastPositions[i], raycastDirection, contactFilter, hitBuffers, raycastDistance);

            if (count != 0)
            {
                //hitBuffersの初期化
                for (int j = 0; j < hitBuffers.Length; j++)
                {
                    hitBuffers[j] = new RaycastHit2D();
                }

                return true;
            }
        }

        //hitBuffersの初期化
        for (int j = 0; j < hitBuffers.Length; j++)
        {
            hitBuffers[j] = new RaycastHit2D();
        }

        return false;
    }

    //キャラクターの進行方向に壁があるか確認する関数
    //敵キャラクターしか使う予定がないためコライダーはboxで計算を行う
    public void CheckWallCollision(int direction)
    {
        Vector2 raycastDirection = new Vector2(direction, 0);
        //Raycastの始点を計算
        Vector2 realSize = box.size * transform.lossyScale;
        Vector2 raycastStart = (Vector2)transform.position + new Vector2(realSize.x * 0.5f * direction, realSize.y * 0.5f);

        raycastPositions[0] = raycastStart + Vector2.up * realSize.y * 0.4f;
        raycastPositions[1] = raycastStart;
        raycastPositions[2] = raycastStart + Vector2.down * realSize.y * 0.4f;

        for(int i = 0; i < raycastPositions.Length; i++)
        {
            int count = Physics2D.Raycast(raycastPositions[i], raycastDirection, contactFilter, hitBuffers, raycastDistanceWall);

            if(count != 0)
            {
                //hitBuffersの初期化
                for (int j = 0; j < hitBuffers.Length; j++)
                {
                    hitBuffers[j] = new RaycastHit2D();
                }

                isWalled = true;
                return;
            }
        }

        //hitBuffersの初期化
        for (int j = 0; j < hitBuffers.Length; j++)
        {
            hitBuffers[j] = new RaycastHit2D();
        }

        isWalled = false;
        return;
    }

    //移動先の地面があるか確認する関数
    public void CheckNextGroundCollision(float direction)
    {
        Vector2 raycastDirection = Vector2.down;
        Vector2 raycasterStart = (Vector2)transform.position + new Vector2(nextGroundedRaycastAdjust_x * direction, groundedRaycastAdjust_y);

        int count = Physics2D.Raycast(raycasterStart, raycastDirection, contactFilter, hitBuffers, raycastDistance);

        Debug.DrawRay(raycasterStart, raycastDirection * raycastDistance, Color.red, 1f);

        if(count != 0)
        {
            //hitBuffersの初期化
            for (int j = 0; j < hitBuffers.Length; j++)
            {
                hitBuffers[j] = new RaycastHit2D();
            }

            isNextGrounded = true;
            return;
        }

        //hitBuffersの初期化
        for (int j = 0; j < hitBuffers.Length; j++)
        {
            hitBuffers[j] = new RaycastHit2D();
        }

        isNextGrounded = false;
        return;
    }

    public bool GetGrounded()
    {
        return isGrounded;
    }

    public bool GetGeilinged()
    {
        return isCeilinged;
    }

    public bool GetWalled()
    {
        return isWalled;
    }

    public bool GetNextGrounded()
    {
        return isNextGrounded;
    }

    /*
    private void OnDrawGizmosSelected()
    {
        var capsule = GetComponent<CapsuleCollider2D>();
        var box = GetComponent<BoxCollider2D>();

        Vector2 size = Vector2.zero;
        float offsetY = 0;

        if (capsule != null)
        {
            size = capsule.size;
            offsetY = capsule.offset.y;
        }
        else if (box != null)
        {
            size = box.size;
            offsetY = box.size.y;
        }

        Vector2 colliderSize = size * transform.lossyScale;

        //Ground
        Handles.color = Color.green;
        Vector2 groundStartBase = (Vector2)transform.position + new Vector2(0, groundedRaycastAdjust_y);

        Vector2[] groundStarts = new Vector2[3];
        groundStarts[0] = groundStartBase + Vector2.left * colliderSize.x * 0.4f;
        groundStarts[1] = groundStartBase;
        groundStarts[2] = groundStartBase + Vector2.right * colliderSize.x * 0.4f;

        foreach (var start in groundStarts)
        {
            Handles.DrawLine(start, start + Vector2.down * raycastDistance);
        }

        //Ceiling
        if (onCeiling)
        {
            Handles.color = Color.cyan;
            Vector2 ceilingStartBase = (Vector2)transform.position +
                new Vector2(0, (offsetY + size.y * 0.5f) * transform.lossyScale.y);

            Vector2[] ceilingStarts = new Vector2[3];
            ceilingStarts[0] = ceilingStartBase + Vector2.left * colliderSize.x * 0.4f;
            ceilingStarts[1] = ceilingStartBase;
            ceilingStarts[2] = ceilingStartBase + Vector2.right * colliderSize.x * 0.4f;

            foreach (var start in ceilingStarts)
            {
                Handles.DrawLine(start, start + Vector2.up * raycastDistance);
            }
        }

        //Wall & NextGround
        var enemy = GetComponent<GroundMovingEnemyPresenter>();
        if (enemy != null && box != null)
        {
            int direction = enemy.direction; // -1 or 1
            Vector2 dirVec = new Vector2(direction, 0);

            Handles.color = Color.red;
            Vector2 realSize = box.size * transform.lossyScale;
            Vector2 wallStartBase = (Vector2)transform.position +
                new Vector2(realSize.x * 0.5f * direction, realSize.y * 0.5f);

            Vector2[] wallStarts = new Vector2[3];
            wallStarts[0] = wallStartBase + Vector2.up * realSize.y * 0.4f;
            wallStarts[1] = wallStartBase;
            wallStarts[2] = wallStartBase + Vector2.down * realSize.y * 0.4f;

            foreach (var start in wallStarts)
            {
                Handles.DrawLine(start, start + dirVec * raycastDistanceWall);
            }

            // NextGround
            Handles.color = Color.magenta;
            Vector2 nextGroundStart = (Vector2)transform.position +
                new Vector2(nextGroundedRaycastAdjust_x * direction, groundedRaycastAdjust_y);
            Handles.DrawLine(nextGroundStart, nextGroundStart + Vector2.down * raycastDistance);
        }

        Handles.color = Color.white;
    }
    */
}