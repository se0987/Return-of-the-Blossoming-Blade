using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene5 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    //private bool flag;
    private bool can = false;
    private bool one1 = true;
    private bool one2 = true;
    private bool one3 = true;

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
        if (can)
        {
            if (one1 && Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(EventCoroutine());
            }
            else if (one2 && Input.GetKeyDown(KeyCode.Q))
            {
                one2 = false;
                StartCoroutine(EventCoroutine());
            }
            else if (one3 && Input.GetKeyDown(KeyCode.W))
            {
                one3 = false;
                StartCoroutine(EventCoroutine());
            }
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        if (one1)
        {
            one1 = false;
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);
        }else if (one2)
        {
            one2 = false;
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);
        }
        else if (one3)
        {
            one3 = false;
            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(() => !theDM.talking);
        }


        theOrder.Move();
    }
}
