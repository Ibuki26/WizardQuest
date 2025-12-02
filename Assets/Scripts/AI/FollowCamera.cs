using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = Camera.main.transform.position;
    }
}
