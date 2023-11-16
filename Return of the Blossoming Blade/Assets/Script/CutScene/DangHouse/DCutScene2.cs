using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    public string doorSound;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private AudioManager theAudio;
    private BGMManager theBGM;

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
        theAudio = FindObjectOfType<AudioManager>();
        theBGM = FindObjectOfType<BGMManager>();
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
        theAudio.Play(doorSound);
        arrow1.SetActive(false);

        theOrder.Appear("BlackScreen", true);
        yield return new WaitForSeconds(4f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }
        yield return new WaitForSeconds(1f);

        theOrder.Appear("BlackScreen", false);
        theAudio.Play(doorSound);
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");
        theOrder.Move("Player", "RIGHT");

        theDM.ShowDialogue(dialogue_2);
        theOrder.Move("JoPeong", "LEFT");
        theOrder.Move("JoPeong", "LEFT");
        theOrder.Move("JoPeong", "LEFT");
        theOrder.Move("JoPeong", "LEFT");
        theOrder.Move("JoPeong", "LEFT");
        theOrder.Move("JoPeong", "LEFT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Appear("Poor", true);
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        theOrder.Move("Poor", "LEFT");
        yield return new WaitUntil(() => !theDM.talking);

        theBGM.Play(1);

        TransferMap[] temp = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].gateName.Equals("GoToDangBo"))
            {
                temp[i].move = true;
                break;
            }
        }
        arrow2.SetActive(true);

        theOrder.Move();
    }
}
