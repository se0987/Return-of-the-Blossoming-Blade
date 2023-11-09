using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public float amplitude = 2f; // 흔들림의 진폭
    public float frequency = 4f; // 흔들림의 주기

    public bool shakeVertical = true; // 흔들림 방향이 세로인지 여부를 결정하는 플래그

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Start()
    {
        originalPosition = transform.position; // 원래 위치를 저장합니다.
    }

    private void Update()
    {
        if (!isShaking)
        {
            StartShaking();
        }
    }

    private void StartShaking()
    {
        isShaking = true;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        while (isShaking)
        {
            // Mathf.Sin 함수를 사용하여 흔들림 효과를 만듭니다.
            float delta = Mathf.Sin(Time.time * frequency) * amplitude;
            Vector3 newPosition = originalPosition;

            // 세로 흔들림이 선택된 경우 y축을 변경하고, 그렇지 않으면 x축을 변경합니다.
            if (shakeVertical)
            {
                newPosition.y += delta;
            }
            else
            {
                newPosition.x += delta;
            }

            transform.position = newPosition;
            yield return null;
        }
    }

    private void OnDisable()
    {
        isShaking = false; // 오브젝트가 비활성화될 때 흔들림을 중지합니다.
        transform.position = originalPosition; // 위치를 초기화합니다.
    }
}
