using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOMCutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    private AudioManager theAudio;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public string fireSound;

    public GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theAudio = FindObjectOfType<AudioManager>();
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
            arrow.SetActive(false);
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);
        theOrder.Turn("Master1", "DOWN");

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move("Player", "LEFT");
        theOrder.Move("Player", "UP");
        theOrder.Action("Player", "AttackH");//Ã»¸íÀÌ Ä®À» ÈÖµÎ¸§
        yield return new WaitForSeconds(1f);
        theAudio.Play(fireSound);
        theOrder.Appear("Fire1", false);//ºÒÀÌ ²¨Áü
        theOrder.Appear("Fire2", false);
        theOrder.Appear("Fire3", false);
        theOrder.Turn("Master1", "UP");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitForSeconds(0.4f);
        theOrder.Move("Player", "UP");//¾ÕÀ¸·Î ÀüÁø
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToOuter2"))
            {
                temp[i].move = true;
                break;
            }
        }

        theOrder.Move();
    }
}
