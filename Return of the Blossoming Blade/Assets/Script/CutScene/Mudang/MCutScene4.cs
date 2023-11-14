using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCutScene4 : MonoBehaviour
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
    public bool end = false;

    public int enemyCount = 8;

    public GameObject arrow1;
    public GameObject arrow2;
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
        if (one && end)
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

        theOrder.Appear("Poor", true);
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT"); 
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("DangBo", "DOWN");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        TransferScene[] temp = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToJongnam"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
        arrow1.SetActive(true);
        arrow2.SetActive(true);
        arrow3.SetActive(true);
    }
}
