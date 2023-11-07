using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene1 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
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
        theChapter = FindObjectOfType<ChapterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        PlayerPrefs.SetInt("havePosion", 0);
        PlayerPrefs.SetInt("maxPosion", 1);
        PlayerPrefs.SetFloat("playerHP", playerStatus.maxHP-30);
        PlayerPrefs.SetFloat("playerMP", playerStatus.maxMP-10);
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theOrder.Turn("CheongJin", "LEFT");
        theChapter.ShowChapter("Chapter 1\n½ÃÀÛ");

        theDM.ShowDialogue(dialogue_1);

        yield return new WaitUntil(() => !theDM.talking);
        //yield return new WaitForSeconds(1f);

        theOrder.Turn("CheongJin", "DOWN");
        theOrder.Move("CheongJin", "DOWN");

        //yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(0.2f);

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        playerStatus.GetPosion(1);

        theOrder.Move();
    }
}
