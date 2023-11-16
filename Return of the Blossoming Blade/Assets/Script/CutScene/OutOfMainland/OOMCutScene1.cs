using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOMCutScene1 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;
    private PlayerStatus playerStatus;

    private bool one = true;

    public GameObject arrow;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theChapter = FindObjectOfType<ChapterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        theDM.OnExitDialogue += HandleExitDialogue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one)
        {
            one = false;
            StartCoroutine(EventCoroutine());
        }
    }

    void HandleExitDialogue()
    {
        Debug.Log("중지");
        stop = true;
        StopCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);
        theChapter.ShowChapter("Chpater2\n새외 방문");
        PlayerPrefs.SetInt("chapter", 2);

        playerStatus.UpgradeMaxPosion();

        theDM.ShowDialogue(dialogue_1);
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move("DangBo2", "UP");
        theOrder.Move("DangBo2", "UP");
        theOrder.Move("DangBo2", "UP");

        theOrder.Move();
        arrow.SetActive(true);
    }
}
