using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JCutScene0 : MonoBehaviour
{
    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;
    private PlayerStatus playerStatus;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theChapter = FindObjectOfType<ChapterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one)
        {
            one = false;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theChapter.ShowChapter("Chapter 4\n¡æ≥≤");
        PlayerPrefs.SetInt("chapter", 4);

        playerStatus.UpgradeMaxPosion();

        theOrder.Move();
    }
}
