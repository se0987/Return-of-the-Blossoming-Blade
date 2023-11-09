using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene5 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public bool first = true;

    public bool enable = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        can = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        can = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (can && one && Input.GetKeyDown(KeyCode.C) && enable)
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

        /*        if (PlayerPrefs.HasKey("ChunSalTime"))
                {
                    if (PlayerPrefs.GetInt("ChunSalTime") < 10)//당보 생존
                    {
                        theDM.ShowDialogue(dialogue_1);
                        yield return new WaitUntil(() => !theDM.talking);

                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");

                        theDM.ShowDialogue(dialogue_2);
                        yield return new WaitUntil(() => !theDM.talking);

                        theOrder.Move("Player", "DOWN");
                        theOrder.Move("Player", "DOWN");
                        theOrder.Move("Player", "DOWN");
                        theOrder.Move("Player", "DOWN");

                        theDM.ShowDialogue(dialogue_3);
                        yield return new WaitUntil(() => !theDM.talking);
                    }
                    else//당보 죽음
                    {
                        theDM.ShowDialogue(dialogue_4);
                        yield return new WaitUntil(() => !theDM.talking);
                        theOrder.Appear("Player", false);
                        theOrder.Action("DangBo", "WAKEUP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");
                        theOrder.Move("Poor", "UP");

                        theDM.ShowDialogue(dialogue_5);
                        yield return new WaitUntil(() => !theDM.talking);
                    }
                }*/
        if (first)//당보 생존
        {
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);

            theOrder.Appear("Poor2", true);
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Turn("Poor2", "LEFT");

            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);

            theOrder.Move("Player", "DOWN");
            theOrder.Move("Player", "DOWN");
            theOrder.Move("Player", "DOWN");
            theOrder.Move("Player", "DOWN");

            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(() => !theDM.talking);
        }
        else//당보 죽음
        {
            theOrder.Appear("Player", false);
            theOrder.Action("DangBo", "NOTDIE");
            theOrder.Action("DangBo", "WAKEUP");
            theDM.ShowDialogue(dialogue_4);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Appear("Poor2", true);
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Move("Poor2", "UP");
            theOrder.Turn("Poor2", "LEFT");

            theDM.ShowDialogue(dialogue_5);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Action("DangBo", "NOTWAKEUP");
            theOrder.Action("DangBo", "DIE");
            theOrder.Appear("Player", true);
        }

        TransferScene[] temp = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToCheongJin"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
    }
}
