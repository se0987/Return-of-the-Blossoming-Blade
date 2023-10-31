using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP = 100f; // 최대 HP
    public float currentHP;   // 현재 HP

    public float maxMP = 50f;  // 최대 MP
    public float currentMP;   // 현재 MP

    private void Start()
    {
        // 게임 시작시 플레이어의 HP와 MP를 최대치로 설정
        currentHP = maxHP;
        currentMP = maxMP;
    }

    // 데미지를 입었을 때 호출되는 함수
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        // HP가 0 미만이면 0으로 설정
        if (currentHP < 0f)
        {
            currentHP = 0f;
            // TODO: 플레이어 사망 로직 구현
        }
    }

    // MP를 사용했을 때 호출되는 함수
    public void UseMP(float amount)
    {
        currentMP -= amount;

        // MP가 0 미만이면 0으로 설정
        if (currentMP < 0f)
        {
            currentMP = 0f;
        }
    }

    // HP와 MP를 회복하는 함수들도 추가할 수 있습니다.
}