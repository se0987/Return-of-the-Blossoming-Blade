using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCutScene3 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private BGMManager theBGM;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public GameObject Gwanghon;
    public GameObject Block;
    public GameObject GwanghonHP;
    public GameObject GwanghonName;

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
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "DOWN");
        theOrder.Move("DangBo", "DOWN");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "DOWN");
        theOrder.Move("DangBo", "DOWN");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");


        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        Gwanghon.SetActive(true);
        Block.SetActive(true);
        GwanghonHP.SetActive(true);
        GwanghonName.SetActive(true);

        theOrder.Move();
    }
}
