using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JCutScene0 : MonoBehaviour
{
    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;
    private PlayerStatus playerStatus;

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
        theChapter = FindObjectOfType<ChapterManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
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

        theChapter.ShowChapter("Chapter 4\n종남");
        PlayerPrefs.SetInt("chapter", 4);

        playerStatus.UpgradeMaxPosion();

        theOrder.Move();
    }
}
