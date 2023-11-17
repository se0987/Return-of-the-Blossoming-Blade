using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint2 : MonoBehaviour
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
            PlayerPrefs.SetString("save2", "제1장");
            save.saveChapter[1].text = "제1장";
        }
        else
        {
            string chapter = PlayerPrefs.GetInt("chapter").ToString();
            PlayerPrefs.SetString("save2", "제" + chapter + "장");
            save.saveChapter[1].text = "제" + chapter + "장";
        }
        string chapterName = GetChapterName();
        PlayerPrefs.SetString("save2Name", chapterName);
        save.saveChapterName[1].text = chapterName;
        string today = DateTime.Now.ToString("yyyy년MM월dd일");
        PlayerPrefs.SetString("save2Date", today);
        save.saveDate[1].text = today;

        save.saveButton[1].SetActive(false);
        save.goSavePoint[1].SetActive(true);

        save.delete[1].SetActive(true);
        save.deleteText[1].text = "X";

        //추가 저장
        PlayerPrefs.SetString("save2SceneName", GetSceneName());
        PlayerPrefs.SetFloat("save2PlayerHP", PlayerPrefs.GetFloat("playerHP"));
        PlayerPrefs.SetFloat("save2PlayerMP", PlayerPrefs.GetFloat("playerMP"));
        PlayerPrefs.SetInt("save2HavePosion", PlayerPrefs.GetInt("havePosion"));
        PlayerPrefs.SetInt("save2MaxPosion", PlayerPrefs.GetInt("maxPosion"));

        PlayerPrefs.SetString("save2MapName", PlayerPrefs.GetString("playerMapName"));
        PlayerPrefs.SetString("save2playerGateName", PlayerPrefs.GetString("playerGateName"));
        PlayerPrefs.SetInt("save2CJEvent2One", PlayerPrefs.GetInt("CJEvent2One"));
        PlayerPrefs.SetInt("save2Choice1", PlayerPrefs.GetInt("choice1"));
        PlayerPrefs.SetInt("save2Choice2", PlayerPrefs.GetInt("choice2"));
        PlayerPrefs.SetInt("save2Choice3", PlayerPrefs.GetInt("choice3"));
        PlayerPrefs.SetFloat("save2JeokCheonPlayTime", PlayerPrefs.GetFloat("JeokCheonPlayTime"));
        PlayerPrefs.SetFloat("save2GwanghonPlayTime", PlayerPrefs.GetFloat("GwanghonPlayTime"));
        PlayerPrefs.SetFloat("save2ChunsalPlayTime", PlayerPrefs.GetFloat("ChunsalPlayTime"));
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
