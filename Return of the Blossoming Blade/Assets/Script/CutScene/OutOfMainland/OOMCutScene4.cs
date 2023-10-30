using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOMCutScene4 : MonoBehaviour
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
        StartCoroutine(EventCoroutine());
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move("Player", "DOWN");
        //theOrder.Move("OOMMaster", "SURPRISE"); //±ôÂ¦ ³î¶ó´Â ¸ð¼Ç

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitForSeconds(0.4f);
        theOrder.Move("Player", "DOWN");
        theOrder.Move("Player", "DOWN");
        yield return new WaitForSeconds(0.5f);
        /*theOrder.Move("OOMMaster", "DOWN");//¾ß¼ö±ÃÀ¸·Î ¾È³»
        theOrder.Move("OOMMaster", "DOWN");
        theOrder.Move("OOMMaster", "DOWN");
        theOrder.Move("OOMMaster", "DOWN");*/
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
    }
}
