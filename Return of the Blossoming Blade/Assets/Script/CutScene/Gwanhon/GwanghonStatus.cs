using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GwanghonStatus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float maxHealth = 1f;
    private float currentHealth;

    public Image bossHpBar;

    public GameObject HP;
    public GameObject Name;

    private bool isDead = false;
    private bool isInvulnerable = false; // 무적 상태를 추적하는 변수

    private MCutScene4 mCutScene4;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        mCutScene4 = FindObjectOfType<MCutScene4>();

        GameObject hpGaugeObject = GameObject.Find("Boss_HP_Gauge1");
        if (hpGaugeObject)
        {
            bossHpBar = hpGaugeObject.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (bossHpBar != null)
        {
            float hpRatio = currentHealth / maxHealth;
            bossHpBar.fillAmount = hpRatio;
        }
    }

    void Die()
    {
        if (!isDead)
        {
            isDead = true;
            GameObject Gwanghon = GameObject.Find("Gwanghon");
            if (Gwanghon != null)
            {
                Gwanghon.SetActive(false);
                bossHpBar.fillAmount = 0f;
                mCutScene4.end = true;
                HP.SetActive(false);
                Name.SetActive(false);
            }
        }
    }

   private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Weapon") && !isInvulnerable)
    {
        WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
        if (weapon)
        {
            if (currentHealth > 0)
            {
                StartCoroutine(FlashCoroutine());
            }

            currentHealth -= weapon.damageAmount;
            if (currentHealth <= 0)
            {
                Die();
            }

            // 활성 상태 확인 후 코루틴 시작
            if(gameObject.activeInHierarchy) 
            {
                StartCoroutine(InvulnerabilityCoroutine());
            }
        }
    }
}

    private IEnumerator FlashCoroutine()
    {
        float flashDuration = 0.1f;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(0.5f); // 0.5초 동안 무적
        isInvulnerable = false;
    }
}