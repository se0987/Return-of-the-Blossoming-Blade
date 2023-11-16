using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP = 100f; // 최대 HP
    public float currentHP;

    public float maxMP = 50f;  // 최대 MP
    public float currentMP;

    public int maxPosion = 1;
    public int currentPosion;

    private AudioManager theAudio;
    public string posionSound;

    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        // 체력 감소
        currentHP -= damage;

        // 체력이 0 이하가 되었는지 확인
        if (currentHP <= 0)
        {
            currentHP = 0; // 체력을 0으로 설정
            PlayerPrefs.SetFloat("playerHP", currentHP);

            // 현재 씬 이름 확인
            string currentSceneName = SceneManager.GetActiveScene().name;

            // 'Chunma' 씬인 경우 DieInCheonma 호출
            if (currentSceneName == "Chunma")
            {
                DieInCheonma();
            }

            StartCoroutine(FlashCoroutine()); // 피해 입은 효과
        }
        else
        {
            // 체력이 0보다 큰 경우에만 PlayerPrefs에 저장
            PlayerPrefs.SetFloat("playerHP", currentHP);
            StartCoroutine(FlashCoroutine()); // 피해 입은 효과
        }
    }
    private IEnumerator FlashCoroutine()
    {
        float flashDuration = 0.1f;

        spriteRenderer.color = Color.red; // 색상을 빨간색으로 변경
        yield return new WaitForSeconds(flashDuration); // 지정된 시간 동안 기다림
        spriteRenderer.color = Color.white; // 원래 색상으로 되돌림
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
                theAudio.Play(posionSound);
                PlayerPrefs.SetInt("havePosion", havePosion - 1);

                float playerHP = PlayerPrefs.GetFloat("playerHP");
                if (playerHP + 30f > maxHP)
                {
                    PlayerPrefs.SetFloat("playerHP", maxHP);
                }
                else
                {
                    PlayerPrefs.SetFloat("playerHP", playerHP + 30f);
                }

                playerHP = PlayerPrefs.GetFloat("playerHP");

                currentHP = playerHP;

                float playerMP = PlayerPrefs.GetFloat("playerMP");
                if (playerMP + 10f > maxMP)
                {
                    PlayerPrefs.SetFloat("playerMP", maxMP);
                }
                else
                {
                    PlayerPrefs.SetFloat("playerMP", playerMP + 10f);
                }

                playerMP = PlayerPrefs.GetFloat("playerMP");

                currentMP = playerMP;
            }
        }
    }

    void DieInCheonma()
    {
        End5.end = true;
        TransferMap[] temp = FindObjectsOfType<TransferMap>();

        GameObject Cheonma = GameObject.Find("Cheonma Bon In");

        DisableCheonmaBehaviors(Cheonma);
        ResetCheonmaAnimator(Cheonma);
        DisableAllClonedSpriteRenderers();

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("EndPoint5"))
            {
                temp[i].GoToPoint();
                break;
            }
        }
        PlayerPrefs.SetFloat("End5", 1);
    }

    void DisableCheonmaBehaviors(GameObject cheonma)
    {
        foreach (var behavior in cheonma.GetComponents<Behaviour>())
        {
            behavior.enabled = false;
        }

        Animator animator = cheonma.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void ResetCheonmaAnimator(GameObject cheonma)
    {
        Animator animator = cheonma.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind();
            animator.enabled = false;
        }
    }

    void DisableAllClonedSpriteRenderers()
    {
        SpriteRenderer[] allSpriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer renderer in allSpriteRenderers)
        {
            if (renderer.gameObject.name.Contains("(Clone)"))
            {
                renderer.enabled = false;
            }
        }
    }
}