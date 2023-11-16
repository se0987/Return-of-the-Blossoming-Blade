using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3AttackCollider : MonoBehaviour
{
    public float SkillDamage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCombatCol"))
        {
            PlayerStatus playerStatus = collision.transform.parent.GetComponent<PlayerStatus>();
            playerStatus.TakeDamage(SkillDamage);
        }
    }
}
