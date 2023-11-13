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
        PlayerPrefs.SetInt("choice2", 0);
        PlayerPrefs.SetInt("choice3", 0);
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
        theOrder.Turn("Player", "DOWN");
        theOrder.Appear("CheongMun", true);
        theOrder.Turn("CheongMun", "RIGHT");
        theChapter.ShowChapter("Chapter6\nÃ»Áø");

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

        theOrder.Move();
    }
}

