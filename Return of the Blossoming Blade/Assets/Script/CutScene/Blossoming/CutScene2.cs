using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;
    public Dialogue dialogue_6;
    public Dialogue dialogue_7;

    public Choice choice_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private CutScene3 cutScene3;

    private List<TransferMap> transferGates = new List<TransferMap>();

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public GameObject arrow1;
    public GameObject arrow2;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        cutScene3 = FindObjectOfType<CutScene3>();
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
        if (one && can && Input.GetKeyDown(KeyCode.C) && PlayerPrefs.GetInt("havePosion") == 0)
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

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        //yield return new WaitForSeconds(1f);

        arrow1.SetActive(false);

        theOrder.Move("CheongJin", "UP");
        theOrder.Move("CheongJin", "UP");
        theOrder.Move("CheongJin", "RIGHT");

        //yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(1f);

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Turn("CheongJin", "DOWN");
        
        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);

        theChoice.ShowChoice(choice_1, 1);
        yield return new WaitUntil(() => !theChoice.talking);
        
        if (PlayerPrefs.HasKey("choice1"))
        {
            if (PlayerPrefs.GetInt("choice1") == 1)
            {
                yield return new WaitForSeconds(0.2f);
                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(() => !theDM.talking);
                theDM.ShowDialogue(dialogue_5);
                theOrder.Move("CheongJin", "RIGHT");
                theOrder.Move("CheongJin", "RIGHT");
                theOrder.Move("CheongJin", "RIGHT");
                theOrder.Move("CheongJin", "UP");
                theOrder.Move("CheongJin", "UP");
                theOrder.Move("CheongJin", "UP");
                theOrder.Move("CheongJin", "UP");
                yield return new WaitForSeconds(1.4f);
                theOrder.Appear("CheongJin", false);
                theOrder.Appear("BlackScreen", true);
                yield return new WaitForSeconds(3f);
                theOrder.Appear("BlackScreen", false);
                yield return new WaitUntil(() => !theDM.talking);
                theDM.ShowDialogue(dialogue_6);
                yield return new WaitUntil(() => !theDM.talking);
                arrow2.SetActive(true);
                theOrder.Appear("CheongMun", true);
            }
            else
            {
                theDM.ShowDialogue(dialogue_7);
                yield return new WaitUntil(() => !theDM.talking);
                arrow2.SetActive(true);
                TransferMap[] temp1 = FindObjectsOfType<TransferMap>();
                for (int i = 0; i < temp1.Length; i++)
                {
                    if (temp1[i].gateName.Equals("GoToTown"))
                    {
                        temp1[i].move = true;
                        break;
                    }
                }
                theOrder.Appear("CheongMun", false);
                cutScene3.enable = false;
            }
        }
        else
        {
                Debug.Log("choice1 값이 없음");
        }

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GetOutInTheRoom") || temp[i].gateName.Equals("GoToTheRoom"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
    }
}
