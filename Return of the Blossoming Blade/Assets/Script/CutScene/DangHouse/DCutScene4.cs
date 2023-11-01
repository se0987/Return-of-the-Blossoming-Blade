using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene4 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    //private bool flag;
    private bool can = false;

    private bool oneStart = true;
    private bool oneFace2 = true;
    private bool oneEnd = true;

    public bool start = false;
    public bool face2 = false;
    public bool end = false;

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
        if(oneStart && start)
        {
            oneStart = false;
            StartCoroutine(EventCoroutine());
        }
        else if (face2 && oneFace2)
        {
            oneFace2 = false;
            StartCoroutine(EventCoroutine());
        }else if (oneEnd && end)
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

        if (start)
        {
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);
            start = false;
        }
        else if (face2)
        {
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);
            face2 = false;
        }
        else if(end)
        {
            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(() => !theDM.talking);
            end = false;
        }

        theOrder.Move();
    }
}
