using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAttack : MonoBehaviour
{
    private EnemyAI enemyAI;  // 부모 오브젝트의 EnemyAI 스크립트에 대한 참조

    private void Start()
    {
        // 부모 오브젝트에서 EnemyAI 컴포넌트를 찾아 참조합니다.
        enemyAI = GetComponentInParent<EnemyAI>();

        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI component not found on parent object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name + " with tag: " + collision.tag);

        // Weapon 태그와의 충돌은 무시
        if (collision.gameObject.CompareTag("Weapon"))
        {
            return;
        }

        // 만약 부딪힌 오브젝트가 PlayerCombatCol 태그를 가지고 있다면
        if (collision.CompareTag("PlayerCombatCol"))
        {
            // PlayerStatus 스크립트를 가져옵니다. (이때 PlayerCombatCol의 부모인 Player 오브젝트로부터 가져옵니다.)
            PlayerStatus playerStatus = collision.transform.parent.GetComponent<PlayerStatus>();

            if (playerStatus)
            {
                // PlayerStatus의 TakeDamage 함수를 호출하여 damageToPlayer 값만큼의 데미지를 줍니다.
                playerStatus.TakeDamage(enemyAI.damageToPlayer);
            }
        }
    }
}