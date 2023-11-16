using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene3 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private PlayerStatus playerStatus;


    //private bool flag;
    private bool can = false;
    private bool one = true;

    public bool enable = true;

    public GameObject arrow3;
    public GameObject arrow4;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        arrow4.SetActive(false);
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
            arrow3.SetActive(false);
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        if (enable)
        {
            theOrder.PreLoadCharacter();
            theOrder.NotMove();
            yield return new WaitForSeconds(0.2f);

            theDM.ShowDialogue(dialogue_1);
            theOrder.Turn("CheongMun", "LEFT");
            yield return new WaitUntil(() => !theDM.talking);
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);
            theOrder.Turn("CheongMun", "UP");

            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(() => !theDM.talking);
            arrow4.SetActive(true);

            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].gateName.Equals("GoToTown"))
                {
                    temp[i].move = true;
                    break;
                }
            }

        theOrder.Move();
        playerStatus.GetPosion(1);
        }
    }
}
