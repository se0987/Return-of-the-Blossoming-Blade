using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End1 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;
    public Dialogue dialogue_6;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;

    public int playMusicTrack1;
    public int playMusicTrack2;
    private BGMManager bgmManager;
    private AudioManager theAudio;
    public string swordSound;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public bool end = false;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        bgmManager = FindObjectOfType<BGMManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theChapter = FindObjectOfType<ChapterManager>();
        theDM.OnExitDialogue += HandleExitDialogue;
    }

    void HandleExitDialogue()
    {
        Debug.Log("중지");
        stop = true;
        StopCoroutine(EventCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one && end)
        {
            one = false;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].gateName.Equals("EndPoint1"))
                {
                    temp[i].move = false;
                    break;
                }
            }
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        bgmManager.Stop();
        bgmManager.Play(playMusicTrack1);

        theOrder.Action("Player", "LAST");

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }

        theDM.ShowDialogue(dialogue_2);
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }

        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }
        theOrder.Action("Player", "AttackH");

        bgmManager.Stop();
        theAudio.Play(swordSound);
        bgmManager.Play(playMusicTrack2);

        theDM.ShowDialogue(dialogue_4);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }
        theOrder.Action("Player", "DIE");

        theDM.ShowDialogue(dialogue_5);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }
        theOrder.Appear("BlackScreen", true);

        theDM.ShowDialogue(dialogue_6);
        yield return new WaitUntil(() => !theDM.talking);

        theChapter.ShowChapter("결말 1\n동귀어진");

        theOrder.Move();
    }
}

