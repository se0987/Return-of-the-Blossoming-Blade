using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJCutScene3 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private CJCutScene4 cjCutScene4;

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
        cjCutScene4 = FindObjectOfType<CJCutScene4>();
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
        if (PlayerPrefs.HasKey("choice2"))
        {
            if (PlayerPrefs.GetInt("choice2") == 1)
            {
                if (one && can && Input.GetKeyDown(KeyCode.C))
                {
                    one = false;
                    StartCoroutine(EventCoroutine());
                }
            }
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Appear("CheongJin", false);

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToCheongMun"))
            {
                temp[i].move = true;
                break;
            }
        }

        cjCutScene4.enable = true;

        theOrder.Move();
    }
}

