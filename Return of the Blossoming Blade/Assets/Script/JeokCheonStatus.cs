using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JeokCheonStatus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float maxHealth = 1f;
    private float currentHealth;

    public Image bossHpBar;
    public GameObject arrow;

    private bool isAttacked = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

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
        GameObject jeokCheon = GameObject.Find("JeokCheon");

        if (jeokCheon != null)
        {
            jeokCheon.SetActive(false);
            bossHpBar.fillAmount = 0;

            GameObject bossName = GameObject.Find("Boss Name");
            if (bossName != null)
            {
                bossName.SetActive(false);
            }
            else
            {
                Debug.LogError("BossName object not found!");
            }

            GameObject bossHpGauge2 = GameObject.Find("Boss_HP_Gauge2");
            if (bossHpGauge2 != null)
            {
                bossHpGauge2.SetActive(false);
            }
            else
            {
                Debug.LogError("Boss_HP_Gauge1 object not found!");
            }
            arrow.SetActive(true);
        }

        GameObject goToOuter3 = GameObject.Find("GoToOuter3");
        if (goToOuter3 != null)
        {
            TransferMap transferMap = goToOuter3.GetComponent<TransferMap>();
            if (transferMap != null)
            {
                transferMap.move = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacked) return;

        if (collision.CompareTag("Weapon") && !isAttacked)
        {
            isAttacked = true;

            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                StartCoroutine(FlashCoroutine());
                StartCoroutine(AttackReset());
        
                currentHealth -= weapon.damageAmount;
            }
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator FlashCoroutine()
    {
        float flashDuration = 0.1f;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        isAttacked = false;
        spriteRenderer.color = Color.white;
    }

    private IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(1.5f);
        isAttacked = false;
    }
}
