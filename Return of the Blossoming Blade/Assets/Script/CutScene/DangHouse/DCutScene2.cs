using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene2 : MonoBehaviour
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

        theOrder.Appear("BlackScreen", true);
        yield return new WaitForSeconds(4f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        yield return new WaitForSeconds(1f);

        theOrder.Appear("BlackScreen", false);
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
    }
}
