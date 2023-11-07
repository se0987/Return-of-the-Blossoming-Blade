using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP = 100f; // 최대 HP
    public float currentHP;

    public float maxMP = 50f;  // 최대 MP
    public float currentMP;

    public int maxPosion = 1;
    public int currentPosion;

    private void Start()
    {
        // 게임 시작시 플레이어의 HP와 MP를 최대치로 설정
        if (!PlayerPrefs.HasKey("playerHP"))
        {
            PlayerPrefs.SetFloat("playerHP", maxHP);
            currentHP = maxHP;
        }
        else
        {
            currentHP = PlayerPrefs.GetFloat("playerHP");
        }

        if (!PlayerPrefs.HasKey("playerMP"))
        {
            PlayerPrefs.SetFloat("playerMP", maxMP);
            currentMP = maxMP;
        }
        else
        {
            currentMP = PlayerPrefs.GetFloat("playerMP");
        }

        if (!PlayerPrefs.HasKey("havePosion"))
        {
            PlayerPrefs.SetInt("havePosion", 0);
            currentPosion = 0;
        }
        else
        {
            currentPosion = PlayerPrefs.GetInt("havePosion");
        }

        if (!PlayerPrefs.HasKey("maxPosion"))
        {
            PlayerPrefs.SetInt("maxPosion", 1);
            maxPosion = 1;
        }
        else
        {
            maxPosion = PlayerPrefs.GetInt("maxPosion");
        }
    }

    // 데미지를 입었을 때 호출되는 함수
    public void TakeDamage(float damage)
    {

        // HP가 0 미만이면 0으로 설정
        if (PlayerPrefs.GetFloat("playerHP") - damage < 0f)
        {
            PlayerPrefs.SetFloat("playerHP", 0);
            currentHP = 0;
            // TODO: 플레이어 사망 로직 구현
        }
        currentHP = PlayerPrefs.GetFloat("playerHP") - damage;
        PlayerPrefs.SetFloat("playerHP", currentHP);
    }

    // MP를 사용했을 때 호출되는 함수
    public void UseMP(float amount)
    {
        if (PlayerPrefs.GetFloat("playerMP") - amount < 0f)
        {
            PlayerPrefs.SetFloat("playerMP", 0);
            currentMP = 0;
            // TODO: 플레이어 사망 로직 구현
        }
        currentMP = PlayerPrefs.GetFloat("playerMP") - amount;
        PlayerPrefs.SetFloat("playerMP", currentMP);
    }

    public void GetPosion(int n)
    {
        if (PlayerPrefs.HasKey("havePosion") && PlayerPrefs.HasKey("maxPosion"))
        {
            int maxPosion = PlayerPrefs.GetInt("maxPosion");
            if (PlayerPrefs.GetInt("havePosion") + n > maxPosion)
            {
                PlayerPrefs.SetInt("havePosion", maxPosion);
                currentPosion = maxPosion;
            }
            else
            {
                currentPosion = PlayerPrefs.GetInt("havePosion") + n;
                PlayerPrefs.SetInt("havePosion", currentPosion);
            }
        }
    }

    public void UpgradeMaxPosion()
    {
        maxPosion += 1;
        PlayerPrefs.SetInt("maxPosion", maxPosion);
        currentPosion = maxPosion;
        PlayerPrefs.SetInt("havePosion", currentPosion);
    }

    public void UsePosion()
    {
        if (PlayerPrefs.HasKey("havePosion"))
        {
            int havePosion = PlayerPrefs.GetInt("havePosion");
            if (havePosion > 0)
            {
                PlayerPrefs.SetInt("havePosion", havePosion - 1);

                int playerHP = PlayerPrefs.GetInt("playerHP");
                PlayerPrefs.SetInt("playerHP", playerHP + 30);
                currentHP = playerHP;

                int playerMP = PlayerPrefs.GetInt("playerMP");
                PlayerPrefs.SetInt("playerMP", playerMP + 10);
                currentMP = playerMP;
            }
        }
    }
}