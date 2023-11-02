using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ResizeCanvas : MonoBehaviour
{
    private Canvas canvas;
    public Camera mainCamera;
    public Vector3 offset = Vector3.zero; // 원하는 경우 카메라로부터의 오프셋을 조정할 수 있습니다.


    private void Start()
    {
        canvas = GetComponent<Canvas>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        Resize();
    }

    private void LateUpdate()
    {
        FollowCamera();
    }

    void FollowCamera()
    {
        // 카메라 위치에 오프셋을 더해 캔버스 위치를 업데이트합니다.
        transform.position = mainCamera.transform.position + offset;
    }


    void Resize()
    {
        // Orthographic 카메라를 가정하였습니다.
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
    }
}