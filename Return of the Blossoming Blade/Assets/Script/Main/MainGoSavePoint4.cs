using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainGoSavePoint4 : MonoBehaviour
{
    private ContinueDialogueManager save;
    private PlayerManager player;
    private CameraManager theCamera;
    private DialogueManager theDialogue;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<ContinueDialogueManager>();
        player = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theDialogue = FindObjectOfType<DialogueManager>();
    }

    public void OnBtnClick()
    {
        theDialogue.ShowLoading();
        theDialogue.StopDialogue();
        //창 닫기
        player.allStop = false;
        player.notMove = false;
        save.CloseDialogue();

        //씬 이동
        TransferScene[] temp1 = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp1.Length; i++)
        {
            if (temp1[i].gateName.Equals(PlayerPrefs.GetString("save4SceneName")))
            {
                temp1[i].GoToScene();
                break;
            }
        }
        //청명 HP, MP
        PlayerPrefs.SetFloat("playerHP", PlayerPrefs.GetFloat("save4PlayerHP"));
        PlayerPrefs.SetFloat("playerMP", PlayerPrefs.GetFloat("save4PlayerMP"));
        //영약
        PlayerPrefs.SetFloat("havePosion", PlayerPrefs.GetFloat("save4HavePosion"));
        PlayerPrefs.SetFloat("maxPosion", PlayerPrefs.GetFloat("save4MaxPosion"));

        //맵 이동
        TransferMap[] temp2 = FindObjectsOfType<TransferMap>();
        for (int i = 0; i < temp2.Length; i++)
        {
            if (temp2[i].gateName.Equals(PlayerPrefs.GetString("save4playerGateName")))
            {
                temp2[i].GoToPoint();
                break;
            }
        }


        //각종 데이터 적용
        PlayerPrefs.SetString("MapName", PlayerPrefs.GetString("save4MapName"));
        PlayerPrefs.SetInt("CJEvent2One", PlayerPrefs.GetInt("save4CJEvent2One"));
        PlayerPrefs.SetInt("Choice1", PlayerPrefs.GetInt("save4Choice1"));
        PlayerPrefs.SetInt("Choice2", PlayerPrefs.GetInt("save4Choice2"));
        PlayerPrefs.SetInt("Choice3", PlayerPrefs.GetInt("save4Choice3"));
        PlayerPrefs.SetFloat("JeokCheonPlayTime", PlayerPrefs.GetFloat("save4JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("GwanghonPlayTime", PlayerPrefs.GetFloat("save4GwanghonPlayTime"));
        PlayerPrefs.SetFloat("ChunsalPlayTime", PlayerPrefs.GetFloat("save4ChunsalPlayTime"));

        theDialogue.UnShowLoading();
    }
}
