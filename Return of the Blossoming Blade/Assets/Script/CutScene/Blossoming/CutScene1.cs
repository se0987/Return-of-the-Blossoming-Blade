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
    private bool stop = false;

    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theDM.OnExitDialogue += HandleExitDialogue;
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChapter = FindObjectOfType<ChapterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        PlayerPrefs.SetInt("havePosion", 0);
        PlayerPrefs.SetInt("maxPosion", 1);
        PlayerPrefs.SetFloat("playerHP", playerStatus.maxHP-30);
        PlayerPrefs.SetFloat("playerMP", playerStatus.maxMP-10);
        StartCoroutine(EventCoroutine());
        if (PlayerPrefs.GetInt("onLoad")==1)
        {
            thePlayer.allStop = false;
            thePlayer.notMove = false;
            PlayerPrefs.SetInt("onLoad", 0);
            //청명 HP, MP
            PlayerPrefs.SetFloat("playerHP", PlayerPrefs.GetFloat("save1PlayerHP"));
            PlayerPrefs.SetFloat("playerMP", PlayerPrefs.GetFloat("save1PlayerMP"));
            //영약
            PlayerPrefs.SetFloat("havePosion", PlayerPrefs.GetFloat("save1HavePosion"));
            PlayerPrefs.SetFloat("maxPosion", PlayerPrefs.GetFloat("save1MaxPosion"));

            //맵 이동
            TransferMap[] temp2 = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp2.Length; i++)
            {
                if (temp2[i].gateName.Equals(PlayerPrefs.GetString("save1playerGateName")))
                {
                    temp2[i].GoToPoint();
                    break;
                }
            }
            //각종 데이터 적용
            PlayerPrefs.SetString("MapName", PlayerPrefs.GetString("save1MapName"));
            PlayerPrefs.SetInt("CJEvent2One", PlayerPrefs.GetInt("save1CJEvent2One"));
            PlayerPrefs.SetInt("Choice1", PlayerPrefs.GetInt("save1Choice1"));
            PlayerPrefs.SetInt("Choice2", PlayerPrefs.GetInt("save1Choice2"));
            PlayerPrefs.SetInt("Choice3", PlayerPrefs.GetInt("save1Choice3"));
            PlayerPrefs.SetFloat("JeokCheonPlayTime", PlayerPrefs.GetFloat("save1JeokCheonPlayTime"));
            PlayerPrefs.SetFloat("GwanghonPlayTime", PlayerPrefs.GetFloat("save1GwanghonPlayTime"));
            PlayerPrefs.SetFloat("ChunsalPlayTime", PlayerPrefs.GetFloat("save1ChunsalPlayTime"));
            theDM.StopDialogue();
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
        theOrder.Turn("CheongJin", "LEFT");
        theChapter.ShowChapter("Chapter 1\n시작");
        PlayerPrefs.SetInt("chapter", 1);

        theDM.ShowDialogue(dialogue_1);

        yield return new WaitUntil(() => !theDM.talking);
        //yield return new WaitForSeconds(1f);

        theOrder.Turn("CheongJin", "DOWN");
        theOrder.Move("CheongJin", "DOWN");

        if (stop)
        {
            yield break;
        }

        //yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(0.2f);
        Debug.Log("실행?");
        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        arrow.SetActive(true);

        playerStatus.GetPosion(1);

        theOrder.Move();
    }
}
