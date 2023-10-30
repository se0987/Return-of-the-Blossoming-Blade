using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOMCutScene3 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

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
        theOrder.Action("Player", "1");//Ã»¸íÀÌ Ä®À» ÈÖµÎ¸§
        yield return new WaitForSeconds(0.4f);
        theOrder.Appear("Fire1", false);//ºÒÀÌ ²¨Áü
        theOrder.Appear("Fire2", false);
        theOrder.Appear("Fire3", false);

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitForSeconds(0.4f);
        theOrder.Move("Player", "UP");//¾ÕÀ¸·Î ÀüÁø
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
    }
}
