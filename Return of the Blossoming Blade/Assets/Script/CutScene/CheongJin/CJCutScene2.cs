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
        PlayerPrefs.SetInt("choice2", 0);
        PlayerPrefs.SetInt("choice3", 0);
        theDM.OnExitDialogue += HandleExitDialogue;
        if (PlayerPrefs.HasKey("CJEvent2One"))
        {
            if (PlayerPrefs.GetInt("CJEvent2One") == 1)
            {
                one = false;
            }
        }
    }

    void HandleExitDialogue()
    {
        Debug.Log("중지");
        stop = true;
        StopCoroutine(EventCoroutine());
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
        if (stop)
        {
            yield break;
        }

        theChoice.ShowChoice(choice_1, 2);
        yield return new WaitUntil(() => !theChoice.talking);
        if (stop)
        {
            yield break;
        }

        if (PlayerPrefs.HasKey("choice2"))
        {
            if (PlayerPrefs.GetInt("choice2") == 1)
            {
                yield return new WaitForSeconds(0.2f);
                theDM.ShowDialogue(dialogue_2);
                yield return new WaitUntil(() => !theDM.talking);
                if (stop)
                {
                    yield break;
                }

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
                if (stop)
                {
                    yield break;
                }

                theChoice.ShowChoice(choice_2, 3);
                yield return new WaitUntil(() => !theChoice.talking);
                if (stop)
                {
                    yield break;
                }

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

