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
        if (can && one && Input.GetKeyDown(KeyCode.C))
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

        if (PlayerPrefs.HasKey("ChunSalTime"))
        {
            if (PlayerPrefs.GetInt("ChunSalTime") < 10)
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
            else
            {
                theDM.ShowDialogue(dialogue_4);
                yield return new WaitUntil(() => !theDM.talking);

                theOrder.Move("Poor", "UP");
                theOrder.Move("Poor", "UP");
                theOrder.Move("Poor", "UP");
                theOrder.Move("Poor", "UP");
                theOrder.Move("Poor", "UP");

                theDM.ShowDialogue(dialogue_5);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        theOrder.Move();
    }
}
