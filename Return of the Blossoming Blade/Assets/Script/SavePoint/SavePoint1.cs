using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint1 : MonoBehaviour
{
    private SaveContinueDialogueManager save;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
    }

    public void OnBtnClick()
    {
        if (!PlayerPrefs.HasKey("chapter"))
        {
            PlayerPrefs.SetInt("chapter", 1);
            PlayerPrefs.SetString("save1", "제1장");
            save.saveChapter[0].text = "제1장";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save1", "제" + chapter + "장");
            save.saveChapter[0].text = "제" + chapter + "장";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save1Name", chapterName);
        save.saveChapterName[0].text = chapterName;
        string today = DateTime.Now.ToString("yyyy년MM월dd일");
        PlayerPrefs.SetString("save1Date", today);
        save.saveDate[0].text = today;

        save.saveButton[0].SetActive(false);
        save.goSavePoint[0].SetActive(true);

        save.delete[0].SetActive(true);
        save.deleteText[0].text = "X";

        //추가 저장
        PlayerPrefs.SetString("save1SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save1PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save1PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetInt("save1HavePosion", PlayerPrefs.GetInt("havePosion"));
        PlayerPrefs.SetInt("save1MaxPosion", PlayerPrefs.GetInt("maxPosion"));

        PlayerPrefs.SetString("save1MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save1playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save1CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save1Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save1Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save1Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save1JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save1GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save1ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
    }

    public string GetSceneName()
    {
        switch (PlayerPrefs.GetInt("chapter"))
        {
            case 1:
                return "BlossomingBlade";
                break;
            case 2:
                return "OutOfMainland";
                break;
            case 3:
                return "Mudang";
                break;
            case 4:
                return "Jongnam";
                break;
            case 5:
                return "DangHouse";
                break;
            case 6:
                return "CheongJin";
                break;
            case 7:
                return "Chunma";
                break;
        }
        return "";
    }

    public string GetChapterName()
    {
        switch (PlayerPrefs.GetInt("chapter"))
        {
            case 1:
                return "시작";
                break;
            case 2:
                return "새외방문";
                break;
            case 3:
                return "무당방문";
                break;
            case 4:
                return "종남방문";
                break;
            case 5:
                return "당가방문";
                break;
            case 6:
                return "최종결전 직전";
                break;
            case 7:
                return "대산혈사";
                break;
        }
        return "";
    }
}
