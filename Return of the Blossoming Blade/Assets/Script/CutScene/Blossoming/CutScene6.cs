using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene6 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;
    public Dialogue dialogue_6;
    public Dialogue dialogue_7;
    public Dialogue dialogue_8;
    public Dialogue dialogue_9;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    public int enemyCount = 8;

    //private bool flag;
    private bool can = false;
    private bool one = true;
    protected bool finish = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
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
        if (can && one && Input.GetKeyDown(KeyCode.C) && enemyCount==0)
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

        if (PlayerPrefs.HasKey("choice1"))
        {
            if (PlayerPrefs.GetInt("choice1") == 1)
            {
                theOrder.Appear("FatherAndSon", true);
                theOrder.Appear("Couple", true);
                theOrder.Action("Couple", "COUPLETOGETHER");

                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(() => !theDM.talking);
                theOrder.Move("CheongJin2", "LEFT");
                theOrder.Move("CheongJin2", "UP");
                theOrder.Move("CheongJin2", "UP");
                theOrder.Move("CheongJin2", "UP");

                theDM.ShowDialogue(dialogue_2);
                yield return new WaitForSeconds(2f);
                theOrder.Appear("Poor", true);
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                yield return new WaitUntil(() => !theDM.talking);

                theDM.ShowDialogue(dialogue_3);
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                yield return new WaitUntil(() => !theDM.talking);

                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(() => !theDM.talking);
            }
            else
            {
                theOrder.Appear("Couple", true);
                theOrder.Action("Couple", "COUPLEALONE");

                theDM.ShowDialogue(dialogue_5);
                theOrder.Move("CheongJin2", "RIGHT");
                theOrder.Move("CheongJin2", "UP");
                theOrder.Move("CheongJin2", "UP");
                theOrder.Move("CheongJin2", "UP");
                yield return new WaitUntil(() => !theDM.talking);

                theDM.ShowDialogue(dialogue_6);
                yield return new WaitForSeconds(0.3f);
                theOrder.Move("Player", "UP");
                yield return new WaitUntil(() => !theDM.talking);
                theOrder.Action("Couple", "COUPLEWITH");

                theDM.ShowDialogue(dialogue_7);
                yield return new WaitUntil(() => !theDM.talking);
                theOrder.Appear("Father", true);
                yield return new WaitForSeconds(0.3f);
                theOrder.Move("Father", "RIGHT");
                theOrder.Move("Father", "RIGHT");
                theOrder.Move("Father", "RIGHT");

                theDM.ShowDialogue(dialogue_8);
                yield return new WaitUntil(() => !theDM.talking);

                theDM.ShowDialogue(dialogue_9);
                theOrder.Appear("Poor", true);
                theOrder.Turn("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                theOrder.Move("Poor", "RIGHT");
                yield return new WaitUntil(() => !theDM.talking);

                theDM.ShowDialogue(dialogue_3);
                yield return new WaitUntil(() => !theDM.talking);
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");
                theOrder.Move("Player", "LEFT", "Running");

                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        else
        {
            Debug.Log("choice1 값이 없음");
        }

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToForest"))
            {
                temp[i].move = true;
                break;
            }
        }


        theOrder.Move();
    }
}
