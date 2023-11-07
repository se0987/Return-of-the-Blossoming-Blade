using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PosionManager : MonoBehaviour
{
    public TextMeshProUGUI count;

    private DialogueManager dialogueManager;
    private PlayerStatus playerStatus;

    private int havePosion = 0;

    // Start is called before the first frame update
    void Start()
    {
        havePosion = PlayerPrefs.GetInt("havePosion");
        Debug.Log(havePosion);
        count.text = havePosion.ToString();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            playerStatus = playerObject.GetComponent<PlayerStatus>();
            if (!playerStatus)
            {
                Debug.LogWarning("Player 오브젝트에 PlayerStatus 스크립트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    // Update is called once per frame
    void Update()
    {
            count.text = PlayerPrefs.GetInt("havePosion").ToString();
    }
}
