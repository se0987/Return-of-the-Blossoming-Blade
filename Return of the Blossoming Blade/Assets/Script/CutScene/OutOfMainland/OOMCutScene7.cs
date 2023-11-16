using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOMCutScene7 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    //private bool flag;
    private bool can = false;
    private bool oneFace2 = true;
    private bool oneEnd = true;

    public bool face2 = false;
    public bool end = false;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theDM.OnExitDialogue += HandleExitDialogue;
    }

    void HandleExitDialogue()
    {
        Debug.Log("ÁßÁö");
        stop = true;
        StopCoroutine(EventCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (face2 && oneFace2)
        {
            oneFace2 = false;
            StartCoroutine(EventCoroutine());
        }else if (end && oneEnd)
        {
            oneEnd = false;
            StartCoroutine(EventCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        can = false;
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        if (face2)
        {
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);
            face2 = false;
        }
        else if(end)
        {
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);
            end = false;

            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].gateName.Equals("GoToOuter3"))
                {
                    temp[i].move = true;
                    break;
                }
            }
        }

        theOrder.Move();
    }
}
