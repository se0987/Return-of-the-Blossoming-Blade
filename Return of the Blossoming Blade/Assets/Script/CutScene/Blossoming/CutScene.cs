using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene: MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    //private bool flag;
    private bool can = false;
    private bool one = true;

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

        theDM.ShowDialogue(dialogue_1);

        //yield return new WaitUntil(() => !theDM.talking);
        yield return new WaitForSeconds(1f);

        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");

        //yield return new WaitUntil(() => thePlayer.queue.Count == 0);
        yield return new WaitForSeconds(1f);

        theDM.ShowDialogue(dialogue_2);
        //yield return new WaitUntil(() => !theDM.talking);
        yield return new WaitForSeconds(1f);

        theOrder.Move();
    }

    /*    private void OnTriggerStay2D(Collider2D collision)
        {
            if(!flag && Input.GetKeyDown(KeyCode.C))
            {
                flag = true;
                StartCoroutine(EventCoroutine());
            }
        }

        IEnumerator EventCoroutine()
        {
            theOrder.PreLoadCharacter();
            theOrder.NotMove();

            theDM.ShowDialogue(dialogue_1);

            //yield return new WaitUntil(() => !theDM.talking);
            yield return new WaitForSeconds(1f);

            theOrder.Move("Player", "RIGHT");
            theOrder.Move("Player", "RIGHT");

            //yield return new WaitUntil(() => thePlayer.queue.Count == 0);
            yield return new WaitForSeconds(1f);

            theDM.ShowDialogue(dialogue_2);
            //yield return new WaitUntil(() => !theDM.talking);
            yield return new WaitForSeconds(1f);

            theOrder.Move();
        }

        // Update is called once per frame
        void Update()
        {

        }*/
}
