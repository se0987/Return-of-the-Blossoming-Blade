using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint1 : MonoBehaviour
{
    private SaveContinueDialogueManager save;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
    }

    public void OnBtnClick()
    {

        //1 기록 삭제
        save.delete[0].SetActive(false);//X버튼 비활성화
        save.deleteText[0].text = "";
        save.goSavePoint[0].SetActive(false);//저장소로 가는 버튼 비활성화
        save.saveButton[0].SetActive(true);//저장 버튼 활성화
        save.saveChapterName[0].text = "저장";//저장 만들기
        save.saveChapter[0].text = "";//입력된 값 삭제
        save.saveDate[0].text = "";
        PlayerPrefs.DeleteKey("save1");
        PlayerPrefs.DeleteKey("save1Name");
        PlayerPrefs.DeleteKey("save1Date");
    }
}
