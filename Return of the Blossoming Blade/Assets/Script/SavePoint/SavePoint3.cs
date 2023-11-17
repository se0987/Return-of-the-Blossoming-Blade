using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint3 : MonoBehaviour
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
            PlayerPrefs.SetString("save3", "제1장");
            save.saveChapter[2].text = "제1장";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save3", "제" + chapter + "장");
            save.saveChapter[2].text = "제" + chapter + "장";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save3Name", chapterName);
        save.saveChapterName[2].text = chapterName;
        string today = DateTime.Now.ToString("yyyy년MM월dd일");
        PlayerPrefs.SetString("save3Date", today);
        save.saveDate[2].text = today;

        save.saveButton[2].SetActive(false);
        save.goSavePoint[2].SetActive(true);

        save.delete[2].SetActive(true);
        save.deleteText[2].text = "X";

        //추가 저장
        PlayerPrefs.SetString("save3SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save3PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save3PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetInt("save3HavePosion", PlayerPrefs.GetInt("havePosion"));
        PlayerPrefs.SetInt("save3MaxPosion", PlayerPrefs.GetInt("maxPosion"));

        PlayerPrefs.SetString("save3MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save3playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save3CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save3Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save3Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save3Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save3JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save3GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save3ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
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
