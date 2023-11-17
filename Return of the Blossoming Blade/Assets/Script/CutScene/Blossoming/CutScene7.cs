using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene7 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

    public int enemyCount = 7;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public GameObject arrow7;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one && enemyCount == 0)
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
        theOrder.Turn("Player", "LEFT");
        theOrder.Turn("Player", "LEFT");
        yield return new WaitForSeconds(0.4f);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogue_4);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Appear("CheongJin", false);

        theDM.ShowDialogue(dialogue_5);
        yield return new WaitUntil(() => !theDM.talking);

        TransferScene[] temp = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToOutOfMainland"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
        arrow7.SetActive(true);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
    }
}
