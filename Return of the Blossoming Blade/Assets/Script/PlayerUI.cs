using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStatus playerStatus;  // PlayerStatus 참조
    public Image hpBar;  // HP 바 이미지
    public Image mpBar;  // MP 바 이미지

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            playerStatus = playerObject.GetComponent<PlayerStatus>();
            if (!playerStatus)
            {
                Debug.LogWarning("Player 오브젝트에 PlayerStatus 스크립트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // "HP_Gauge1" 오브젝트 찾기 후 hpBar에 할당
        GameObject hpGaugeObject = GameObject.Find("HP_Gauge1");
        if (hpGaugeObject)
        {
            hpBar = hpGaugeObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("HP_Gauge1 오브젝트를 찾을 수 없습니다.");
        }

        // "MP_Gauge1" 오브젝트 찾기 후 mpBar에 할당
        GameObject mpGaugeObject = GameObject.Find("MP_Gauge1");
        if (mpGaugeObject)
        {
            mpBar = mpGaugeObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogWarning("MP_Gauge1 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (playerStatus)
        {
            // HP, MP 비율 계산
            float hpRatio = PlayerPrefs.GetInt("playerHP") / playerStatus.maxHP;
            float mpRatio = PlayerPrefs.GetInt("playerMP") / playerStatus.maxMP;

            // UI 업데이트
            hpBar.fillAmount = hpRatio;
            mpBar.fillAmount = mpRatio;
        }
    }
}