using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJCutScene5 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private CJCutScene4 cjCutScene4;
    public GameObject arrow1;
    public GameObject arrow2;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        cjCutScene4 = FindObjectOfType<CJCutScene4>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerPrefs.HasKey("choice3"))
        {
            if (PlayerPrefs.GetInt("choice3") == 1)
            {
                if (one)
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
        theOrder.Appear("CheongJin", false);
        arrow1.SetActive(false);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

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
        arrow2.SetActive(true);
        theOrder.Move();
    }
}

