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
        if (one && can && Input.GetKeyDown(KeyCode.C))
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

        theChoice.ShowChoice(choice_1);
        yield return new WaitUntil(() => !theChoice.talking);
        
        if (PlayerPrefs.HasKey("choice1"))
        {
            if (PlayerPrefs.GetInt("choice1") == 1)
            {
                yield return new WaitForSeconds(0.2f);
                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(() => !theDM.talking);
                theDM.ShowDialogue(dialogue_5);
                theOrder.Move("CheongJin", "UP");
                yield return new WaitUntil(() => !theDM.talking);
                theDM.ShowDialogue(dialogue_6);
                yield return new WaitUntil(() => !theDM.talking);
            }
            else
            {
                theDM.ShowDialogue(dialogue_7);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        else
        {
                Debug.Log("choice1 값이 없음");
        }

        theOrder.Move();
    }
}
