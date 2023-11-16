using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private BGMManager theBGM;

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
        theBGM = FindObjectOfType<BGMManager>();
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

        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        yield return new WaitForSeconds(1f);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");

        theDM.ShowDialogue(dialogue_1);
        theBGM.Play(1, 0.3f);
        yield return new WaitUntil(() => !theDM.talking);
        //청명이 장로 때리기 가능? 불가능
        theDM.ShowDialogue(dialogue_2);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        yield return new WaitForSeconds(1f);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "LEFT");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
    }
}
