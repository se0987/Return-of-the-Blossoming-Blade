using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{ // 진동할 카메라의 transform
    public Transform shakeCamera;
    // 회전시킬 것인지를 판단할 변수
    public bool shakeRotate = false;
    // 초기 좌표와 회전값을 저장할 변수
    public Vector3 originPos;
    public Quaternion originRot;

 // Use this for initialization
 void Start () {
        // 초깃값 저장
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
 }

    public IEnumerator Shake(float duration = 0.05f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        // 지나간 시간을 누적할 변수
        float passTime = 0.0f;
        // 진동시간동안 루프 돌림
        while(passTime < duration)
        {
            // 불규칙한 위치를 산출
            Vector3 shakePos = Random.insideUnitCircle ;
            // 카메라의 위치를 변경
            shakePos.z = originPos.z / magnitudePos;
            shakeCamera.localPosition = shakePos * magnitudePos;
            
            // 불규칙한 회전을 사용할 경우
            if (shakeRotate)
            {
                // 펄린노이즈함수로 불규칙한 회전값 생성
                Vector3 shakeRot = new Vector3(0,0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));
                // 카메라 회전값 변경
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }

            // 진동시간 누적
            passTime += Time.deltaTime;
            yield return null;
        }
        // 진동 후 원상복구
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;

    }

}