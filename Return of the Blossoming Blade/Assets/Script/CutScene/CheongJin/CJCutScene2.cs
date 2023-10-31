using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJCutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;

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
        can = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        can = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (one && can && Input.GetKeyDown(KeyCode.C))
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

        theOrder.Move();
    }
}
