using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End3 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;
    public Dialogue dialogue_6;

    public int playMusicTrack1;
    public int playMusicTrack2;
    public string swordSound;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;

    private BGMManager bgmManager;
    private AudioManager theAudio;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public bool end = false;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one && end)
        {
            one = false;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].gateName.Equals("EndPoint3"))
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
        bgmManager.Play(playMusicTrack2);

        theOrder.Action("Player", "LAST");

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogue_2);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Action("Player", "AttackH");

        bgmManager.Stop();
        theAudio.Play(swordSound);
        bgmManager.Play(playMusicTrack2);

        theDM.ShowDialogue(dialogue_4);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Action("Player", "DIE");
        theDM.ShowDialogue(dialogue_5);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Appear("BlackScreen", true);
        theDM.ShowDialogue(dialogue_6);
        yield return new WaitUntil(() => !theDM.talking);

        theChapter.ShowChapter("결말 3\n천려일득");

        theOrder.Move();
    }
}

