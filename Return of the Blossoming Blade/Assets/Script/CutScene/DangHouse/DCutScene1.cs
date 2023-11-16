using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene1 : MonoBehaviour
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

    public GameObject arrow1;
    public GameObject arrow2;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
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
        arrow1.SetActive(false);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }
        theOrder.Move("JoPeong", "DOWN");
        theOrder.Move("JoPeong", "DOWN");
        theOrder.Move("JoPeong", "DOWN");
        theOrder.Move("JoPeong", "DOWN");
        theOrder.Move("JoPeong", "DOWN");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        arrow2.SetActive(true);
        theOrder.Move();
    }
}
