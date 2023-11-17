using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJCutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;

    public Choice choice_1;
    public Choice choice_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;
    private PlayerStatus playerStatus;

    public GameObject arrow1;

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
        PlayerPrefs.SetFloat("playerHP", playerStatus.maxHP);
        PlayerPrefs.SetFloat("playerMP", playerStatus.maxMP);
        PlayerPrefs.SetInt("choice2", 0);
        PlayerPrefs.SetInt("choice3", 0);
        int saveNum = PlayerPrefs.GetInt("onLoad");
        if (PlayerPrefs.HasKey("onLoad") && saveNum != 0)
        {
            thePlayer.allStop = false;
            thePlayer.notMove = false;
            PlayerPrefs.SetInt("onLoad", 0);
            //청명 HP, MP
            PlayerPrefs.SetFloat("playerHP", PlayerPrefs.GetFloat("save" + saveNum + "PlayerHP"));
            PlayerPrefs.SetFloat("playerMP", PlayerPrefs.GetFloat("save" + saveNum + "PlayerMP"));
            //영약
            PlayerPrefs.SetFloat("havePosion", PlayerPrefs.GetFloat("save" + saveNum + "HavePosion"));
            PlayerPrefs.SetFloat("maxPosion", PlayerPrefs.GetFloat("save" + saveNum + "MaxPosion"));

            //맵 이동
            TransferMap[] temp2 = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp2.Length; i++)
            {
                if (temp2[i].gateName.Equals(PlayerPrefs.GetString("save" + saveNum + "playerGateName")))
                {
                    temp2[i].GoToPoint();
                    break;
                }
            }
            //각종 데이터 적용
            PlayerPrefs.SetString("MapName", PlayerPrefs.GetString("save" + saveNum + "MapName"));
            PlayerPrefs.SetInt("CJEvent2One", PlayerPrefs.GetInt("save" + saveNum + "CJEvent2One"));
            PlayerPrefs.SetInt("Choice1", PlayerPrefs.GetInt("save" + saveNum + "Choice1"));
            PlayerPrefs.SetInt("Choice2", PlayerPrefs.GetInt("save" + saveNum + "Choice2"));
            PlayerPrefs.SetInt("Choice3", PlayerPrefs.GetInt("save" + saveNum + "Choice3"));
            PlayerPrefs.SetFloat("JeokCheonPlayTime", PlayerPrefs.GetFloat("save" + saveNum + "JeokCheonPlayTime"));
            PlayerPrefs.SetFloat("GwanghonPlayTime", PlayerPrefs.GetFloat("save" + saveNum + "GwanghonPlayTime"));
            PlayerPrefs.SetFloat("ChunsalPlayTime", PlayerPrefs.GetFloat("save" + saveNum + "ChunsalPlayTime"));
            theDM.StopDialogue();
            if (PlayerPrefs.HasKey("CJEvent2One"))
            {
                if (PlayerPrefs.GetInt("CJEvent2One") == 1)
                {
                    one = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one)
        {
            PlayerPrefs.SetInt("CJEvent2One", 2);
            one = false;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);
        theOrder.Turn("Player", "DOWN");
        theOrder.Appear("CheongMun", true);
        theOrder.Turn("CheongMun", "RIGHT");
        theChapter.ShowChapter("Chapter6\n청진");
        PlayerPrefs.SetInt("chapter", 6);

        playerStatus.UpgradeMaxPosion();

        theDM.ShowDialogue(dialogue_1);
        theOrder.Turn("CheongMun", "UP");
        yield return new WaitUntil(() => !theDM.talking);

        theChoice.ShowChoice(choice_1, 2);
        yield return new WaitUntil(() => !theChoice.talking);

        if (PlayerPrefs.HasKey("choice2"))
        {
            if (PlayerPrefs.GetInt("choice2") == 1)
            {
                yield return new WaitForSeconds(0.2f);
                theDM.ShowDialogue(dialogue_2);
                yield return new WaitUntil(() => !theDM.talking);

                TransferMap[] temp = FindObjectsOfType<TransferMap>();
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i].gateName.Equals("GoToCheongJinRoad"))
                    {
                        temp[i].move = true;
                        break;
                    }
                }
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
                theDM.ShowDialogue(dialogue_3);
                yield return new WaitUntil(() => !theDM.talking);

                theChoice.ShowChoice(choice_2, 3);
                yield return new WaitUntil(() => !theChoice.talking);

                if (PlayerPrefs.HasKey("choice3"))
                {
                    if (PlayerPrefs.GetInt("choice3") == 1)
                    {
                        yield return new WaitForSeconds(0.2f);
                        theDM.ShowDialogue(dialogue_4);
                        yield return new WaitUntil(() => !theDM.talking);

                        TransferMap[] temp = FindObjectsOfType<TransferMap>();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].gateName.Equals("GoToCheongJinRoad"))
                            {
                                temp[i].move = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.2f);
                        theDM.ShowDialogue(dialogue_5);
                        yield return new WaitUntil(() => !theDM.talking);

                        TransferMap[] temp = FindObjectsOfType<TransferMap>();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i].gateName.Equals("GoToChunma"))
                            {
                                temp[i].move = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
        }

        arrow1.SetActive(true);
        PlayerPrefs.SetInt("CJEvent2One", 1);

        theOrder.Move();
    }
}

