using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint5 : MonoBehaviour
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
            PlayerPrefs.SetString("save5", "제1장");
            save.saveChapter[4].text = "제1장";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save5", "제" + chapter + "장");
            save.saveChapter[4].text = "제" + chapter + "장";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save5Name", chapterName);
        save.saveChapterName[4].text = chapterName;
        string today = DateTime.Now.ToString("yyyy년MM월dd일");
        PlayerPrefs.SetString("save5Date", today);
        save.saveDate[4].text = today;

        save.saveButton[4].SetActive(false);
        save.goSavePoint[4].SetActive(true);

        save.delete[4].SetActive(true);
        save.deleteText[4].text = "X";

        //추가 저장
        PlayerPrefs.SetString("save5SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save5PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save5PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetInt("save5HavePosion", PlayerPrefs.GetInt("havePosion"));
        PlayerPrefs.SetInt("save5MaxPosion", PlayerPrefs.GetInt("maxPosion"));

        PlayerPrefs.SetString("save5MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save5playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save5CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save5Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save5Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save5Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save5JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save5GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save5ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
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
