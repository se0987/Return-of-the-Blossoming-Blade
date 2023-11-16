using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3AttackCollider : MonoBehaviour
{
    public float SkillDamage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"보스 공격 콜라이드 로직 1");

        if (collision.CompareTag("PlayerCombatCol"))
        {
            Debug.Log($"보스 공격 콜라이드 로직 2");

            // PlayerStatus 스크립트를 가져옵니다. (이때 PlayerCombatCol의 부모인 Player 오브젝트로부터 가져옵니다.)
            PlayerStatus playerStatus = collision.transform.parent.GetComponent<PlayerStatus>();

            playerStatus.TakeDamage(SkillDamage);
        }
    }
}
