using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : MonoBehaviour
{
    private bool hasCollided = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {   
                
                playerStatus.TakeDamage(5.0f);
                hasCollided = true;
                StartCoroutine(ApplyDotDamage(playerStatus, 3.0f, 1.0f, 5));
            }
        }
    }

    IEnumerator ApplyDotDamage(PlayerStatus playerStatus, float damage, float delay, int times)
    {
        for (int i = 0; i < times; i++)
        {
            yield return new WaitForSeconds(delay);
            playerStatus.TakeDamage(damage);
        }
    }
}
