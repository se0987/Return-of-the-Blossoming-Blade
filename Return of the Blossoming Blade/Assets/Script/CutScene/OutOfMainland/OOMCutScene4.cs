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

    public GameObject arrow3;

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

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move("Player", "DOWN");
        theOrder.Move("Player", "DOWN");
        theOrder.Move("Player", "DOWN");
        //theOrder.Move("OOMMaster", "SURPRISE"); //±ôÂ¦ ³î¶ó´Â ¸ð¼Ç

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitForSeconds(1f);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "DOWN");
        theOrder.Move("Player", "DOWN");
        theOrder.Move("Player", "DOWN");
        yield return new WaitForSeconds(3f);
        theOrder.Turn("Player", "RIGHT");
        theOrder.Move("Master2", "DOWN");//¾ß¼ö±ÃÀ¸·Î ¾È³»
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("Master2", "DOWN"); 
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("Master2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        theOrder.Move("DangBo2", "DOWN");
        yield return new WaitUntil(() => !theDM.talking);

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToTheRoom"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
        arrow3.SetActive(true);
    }
}
