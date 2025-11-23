using UnityEngine;
using UnityEditor;

public class CameraViewDrawer : MonoBehaviour
{
    /*
    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;

        // カメラのサイズ（Orthographicの場合は縦の半分のサイズ）
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        // カメラの中心位置
        Vector3 camPos = cam.transform.position;

        // 画面の矩形範囲
        float minX = -camWidth / 2f;
        float maxX = camWidth / 2f;
        float minY = -camHeight / 2f;
        float maxY = camHeight / 2f;

        Vector3 leftTop = transform.position + new Vector3(minX, maxY, 0);
        Vector3 rightTop = transform.position + new Vector3(maxX, maxY, 0);
        Vector3 rightBottom = transform.position + new Vector3(maxX, minY, 0);
        Vector3 leftBottom = transform.position + new Vector3(minX, minY, 0);

        Handles.color = Color.red;
        Handles.DrawLine(leftTop, rightTop);
        Handles.DrawLine(rightTop, rightBottom);
        Handles.DrawLine(rightBottom, leftBottom);
        Handles.DrawLine(leftBottom, leftTop);
        Handles.color = Color.white;
    }
    */
}
