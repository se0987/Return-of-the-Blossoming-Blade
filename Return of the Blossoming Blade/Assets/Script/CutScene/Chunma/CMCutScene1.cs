using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCutScene1 : MonoBehaviour
{
    public Dialogue dialogue_1;

    public int playMusicTrack;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private PlayerStatus playerStatus;
    private BGMManager bgmManager;
    private ChapterManager theChapter;

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
        playerStatus = FindObjectOfType<PlayerStatus>();
        bgmManager = FindObjectOfType<BGMManager>();
        theChapter = FindObjectOfType<ChapterManager>();
        PlayerPrefs.SetFloat("playerHP", playerStatus.maxHP);
        PlayerPrefs.SetFloat("playerMP", playerStatus.maxMP);
        int saveNum = PlayerPrefs.GetInt("onLoad");
        if (saveNum != 0)
        {
            thePlayer.allStop = false;
            thePlayer.notMove = false;
            PlayerPrefs.SetInt("onLoad", 0);
            //청명 HP, MP
            PlayerPrefs.SetFloat("playerHP", PlayerPrefs.GetFloat("save" + saveNum + "PlayerHP"));
            PlayerPrefs.SetFloat("playerMP", PlayerPrefs.GetFloat("save" + saveNum + "PlayerMP"));
            //영약
            PlayerPrefs.SetInt("havePosion", PlayerPrefs.GetInt("save" + saveNum + "HavePosion"));
            PlayerPrefs.SetInt("maxPosion", PlayerPrefs.GetInt("save" + saveNum + "MaxPosion"));

            //맵 이동
            TransferMap[] temp2 = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp2.Length; i++)
            {
                if (temp2[i].gateName.Equals(PlayerPrefs.GetString("save1playerGateName")))
                {
                    temp2[i].GoToPoint();
                    break;
                }
            }
            //각종 데이터 적용
            PlayerPrefs.SetString("MapName", PlayerPrefs.GetString("save" + saveNum + "MapName"));
            PlayerPrefs.SetInt("CJEvent2One", PlayerPrefs.GetInt("save" + saveNum + "CJEvent2One"));
            PlayerPrefs.SetInt("Choice1", PlayerPrefs.GetInt("save" + saveNum + "Choice1"));
            PlayerPrefs.SetInt("Choice2", PlayerPrefs.GetInt("save" + saveNum + "Choice2"));
            PlayerPrefs.SetInt("Choice3", PlayerPrefs.GetInt("save" + saveNum + "Choice3"));
            PlayerPrefs.SetFloat("JeokCheonPlayTime", PlayerPrefs.GetFloat("save" + saveNum + "JeokCheonPlayTime"));
            PlayerPrefs.SetFloat("GwanghonPlayTime", PlayerPrefs.GetFloat("save" + saveNum + "GwanghonPlayTime"));
            PlayerPrefs.SetFloat("ChunsalPlayTime", PlayerPrefs.GetFloat("save" + saveNum + "ChunsalPlayTime"));
            theDM.StopDialogue();
        }
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

        theChapter.ShowChapter("Chapter7\n결전");
        PlayerPrefs.SetInt("chapter", 7);

        theOrder.Appear("BlackScreen", true);
        if (PlayerPrefs.HasKey("choice2"))
        {
            if (PlayerPrefs.GetInt("choice2") == 1)
            {
                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        if (PlayerPrefs.HasKey("choice3"))
        {
            if (PlayerPrefs.GetInt("choice3") == 1)
            {
                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(() => !theDM.talking);
            }
        }
        if(PlayerPrefs.HasKey("choice2") && PlayerPrefs.HasKey("choice3"))
        {
            if(PlayerPrefs.GetInt("choice2") == 0 && PlayerPrefs.GetInt("choice3") == 0)
            {
                playerStatus.UpgradeMaxPosion();
            }
        }
        theOrder.Appear("BlackScreen", false);
        bgmManager.Stop();
        bgmManager.Play(playMusicTrack);

        theOrder.Move();
    }
}

