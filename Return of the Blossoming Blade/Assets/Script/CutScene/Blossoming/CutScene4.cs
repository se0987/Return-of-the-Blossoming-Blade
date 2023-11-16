using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene4 : MonoBehaviour
{
    public Dialogue dialogue_0;
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    private CutScene5 cutScene5;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        cutScene5 = FindObjectOfType<CutScene5>();
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

        theDM.ShowDialogue(dialogue_0);

        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }

        theOrder.Move("CheongJin2", "UP");
        theOrder.Move("CheongJin2", "UP");
        theOrder.Move("CheongJin2", "UP");
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");

        theDM.ShowDialogue(dialogue_1);

        yield return new WaitUntil(() => !theDM.talking);

        cutScene5.enable = true;

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToTown"))
            {
                temp[i].move = false;
                break;
            }
        }

        theOrder.Move();
    }
}
