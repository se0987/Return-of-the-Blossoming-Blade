using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JCutScene1 : MonoBehaviour
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
    
    public GameObject arrow;

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
        arrow.SetActive(false);

        int fame = 0;
        int nigritude = 0;

        if (PlayerPrefs.HasKey("fame"))
        {
            fame = PlayerPrefs.GetInt("fame");
        }

        if (PlayerPrefs.HasKey("nigritude"))
        {
            nigritude = PlayerPrefs.GetInt("nigritude");
        }

        if(fame>1000 && nigritude < 100)
        {
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Appear("JStudent", true);
            theOrder.Move("JStudent", "DOWN");
            theOrder.Move("JStudent", "DOWN");
            theOrder.Move("JStudent", "DOWN");

            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Appear("JTeacher", true);
            theOrder.Move("JTeacher", "DOWN");
            theOrder.Move("JTeacher", "DOWN");
            theOrder.Move("JTeacher", "DOWN");
            theOrder.Move("JTeacher", "DOWN");

            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(() => !theDM.talking);
        }
        else
        {
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Appear("JStudent", true);
            theOrder.Move("JStudent", "DOWN");
            theOrder.Move("JStudent", "DOWN");
            theOrder.Move("JStudent", "DOWN");

            theDM.ShowDialogue(dialogue_4);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Appear("JTeacher", true);
            theOrder.Move("JTeacher", "DOWN");
            theOrder.Move("JTeacher", "DOWN");
            theOrder.Move("JTeacher", "DOWN");

            theDM.ShowDialogue(dialogue_5);
            yield return new WaitUntil(() => !theDM.talking);
        }

        TransferScene[] temp = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToDangHouse"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
    }
}
