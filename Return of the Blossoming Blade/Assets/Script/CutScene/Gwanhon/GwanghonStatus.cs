using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwanghonStatus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float maxHealth = 1f;
    private float currentHealth;

    // public Image bossHpBar;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        GameObject hpGaugeObject = GameObject.Find("Boss_HP_Gauge1");
        if (hpGaugeObject)
        {

            // bossHpBar = hpGaugeObject.GetComponent<Image>();
        }
    }

    private void Update()
    {
        // if (bossHpBar != null)
        {
            float hpRatio = currentHealth / maxHealth;

            // bossHpBar.fillAmount = hpRatio;
        }
    }

void Die()
    {
        Debug.Log("������ �׾����ϴ�.");
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                StartCoroutine(FlashCoroutine());
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
        spriteRenderer.color = Color.white;
    }
}

