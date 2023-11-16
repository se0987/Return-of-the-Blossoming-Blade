using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheonmaStatus : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public float maxHealth = 50f;
    private float currentHealth;

    public Image bossHpBar;

    private bool isDead = false;

    private float remainHealth;
    private float fame;
    private float nigritude;
    private int isDangBo;
    private bool isCheongJin;

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
        if (bossHpBar != null && !isDead)
        {
            float hpRatio = currentHealth / maxHealth;
            bossHpBar.fillAmount = hpRatio;
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

    void Die()
    {
        if (!isDead)
        {
            isDead = true;
            GameObject Cheonma = GameObject.Find("Cheonma Bon In");
            if (Cheonma != null)
            {
                bossHpBar.fillAmount = 0f;
                DisableCheonmaBehaviors(Cheonma);
                ResetCheonmaAnimator(Cheonma);
                DisableAllClonedSpriteRenderers();
            }

            ShowEnding();

        }
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

    void ShowEnding()
    {
        remainHealth = PlayerPrefs.GetFloat("playerHP");
        fame = PlayerPrefs.GetFloat("fame");
        nigritude = PlayerPrefs.GetFloat("nigritude");
        isDangBo = PlayerPrefs.GetInt("DangBoSave");
        isCheongJin = PlayerPrefs.GetInt("choice2") == 2 && PlayerPrefs.GetInt("choice3") == 2;

        if (remainHealth >= 5 && fame >= 250 && isDangBo == 0)
        {
            End2.end = true;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
               if (temp[i].gateName.Equals("EndPoint2"))
                 {
                      temp[i].GoToPoint();
                      break;
                  }
             }
            PlayerPrefs.SetFloat("End2", 1);
        }
        else if (0 < remainHealth && remainHealth < 5 && fame >= 250 && isDangBo == 0)
        {
            End3.end = true;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
               if (temp[i].gateName.Equals("EndPoint3"))
                {
                    temp[i].GoToPoint();
                    break;
                }
            }
            PlayerPrefs.SetFloat("End3", 1);
        }
        else if (0 < remainHealth && remainHealth < 5 && fame >= 300 && isDangBo == 1)
        {
            End4.end = true;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
               if (temp[i].gateName.Equals("EndPoint4"))
                {
                    temp[i].GoToPoint();
                    break;
                }
            }
            PlayerPrefs.SetFloat("End4", 1);
        }
        else if (remainHealth >= 90 && fame >= 325 && isDangBo == 1)
        {
            End6.end = true;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
               if (temp[i].gateName.Equals("EndPoint6"))
                {
                    temp[i].GoToPoint();
                    break;
                }
            }
            PlayerPrefs.SetFloat("End6", 1);
        }
        else
        {
            End1.end = true;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
               if (temp[i].gateName.Equals("EndPoint1"))
                {
                    temp[i].GoToPoint();
                    break;
                }
            }
            PlayerPrefs.SetFloat("End1", 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && collision.CompareTag("Weapon"))
        {
            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                StartCoroutine(FlashCoroutine());
                currentHealth -= weapon.damageAmount;
                currentHealth = Mathf.Max(currentHealth, 0f);
                if (currentHealth <= 0.01f)
                {
                    Die();
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
}
