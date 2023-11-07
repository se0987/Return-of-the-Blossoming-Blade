using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerTransform; 
    public Vector3 offset;         

    void LateUpdate()
    {
        // X, Y 좌표만 플레이어를 따라가도록
        Vector3 newCameraPosition = new Vector3(playerTransform.position.x + offset.x, playerTransform.position.y + offset.y, transform.position.z);
        transform.position = newCameraPosition;
    }
}
