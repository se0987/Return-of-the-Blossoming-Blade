using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GoSavePoint1 : MonoBehaviour
{
    private SaveContinueDialogueManager save;
    private PlayerManager player;
    private CameraManager theCamera;
    private DialogueManager theDialogue;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
        player = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theDialogue = FindObjectOfType<DialogueManager>();
    }

    public void OnBtnClick()
    {
        Debug.Log("버튼 클릭?");
        theDialogue.ShowLoading();
        theDialogue.StopDialogue();

        //씬 이동
        TransferScene[] temp1 = FindObjectsOfType<TransferScene>();
        for (int i = 0; i < temp1.Length; i++)
        {
            if (temp1[i].gateName.Equals(PlayerPrefs.GetString("save1SceneName")))
            {
                temp1[i].GoToScene();
                break;
            }
        }
        //창 닫기
        player.allStop = false;
        player.notMove = false;
        save.CloseSaveDialogue();
        //청명 HP, MP
        PlayerPrefs.SetFloat("playerHP", PlayerPrefs.GetFloat("save1PlayerHP"));
        PlayerPrefs.SetFloat("playerMP", PlayerPrefs.GetFloat("save1PlayerMP"));
        //영약
        PlayerPrefs.SetFloat("havePosion", PlayerPrefs.GetFloat("save1HavePosion"));
        PlayerPrefs.SetFloat("maxPosion", PlayerPrefs.GetFloat("save1MaxPosion"));

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
        PlayerPrefs.SetString("MapName", PlayerPrefs.GetString("save1MapName"));
        PlayerPrefs.SetInt("CJEvent2One", PlayerPrefs.GetInt("save1CJEvent2One"));
        PlayerPrefs.SetInt("Choice1", PlayerPrefs.GetInt("save1Choice1"));
        PlayerPrefs.SetInt("Choice2", PlayerPrefs.GetInt("save1Choice2"));
        PlayerPrefs.SetInt("Choice3", PlayerPrefs.GetInt("save1Choice3"));
        PlayerPrefs.SetFloat("JeokCheonPlayTime", PlayerPrefs.GetFloat("save1JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("GwanghonPlayTime", PlayerPrefs.GetFloat("save1GwanghonPlayTime"));
        PlayerPrefs.SetFloat("ChunsalPlayTime", PlayerPrefs.GetFloat("save1ChunsalPlayTime"));

        theDialogue.UnShowLoading();
    }
}
