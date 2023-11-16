using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothHand : MonoBehaviour
{
    private bool hasCollided = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasCollided && other.gameObject.CompareTag("Player"))
        {

            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.TakeDamage(10.0f);
                hasCollided = true;
            }
        }
    }
}
