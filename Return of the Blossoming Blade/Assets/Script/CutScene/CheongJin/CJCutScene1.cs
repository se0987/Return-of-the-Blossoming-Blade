using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJCutScene1 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    public GameObject arrow1;

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
        arrow1.SetActive(false);
        theOrder.Appear("CheongMun", false);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToCheongJinRoad"))
            {
                temp[i].move = false;
                break;
            }
        }

        theOrder.Move();
    }
}
