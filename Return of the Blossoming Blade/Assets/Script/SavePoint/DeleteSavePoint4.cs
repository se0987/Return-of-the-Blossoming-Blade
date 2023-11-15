using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint4 : MonoBehaviour
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
        save.delete[3].SetActive(false);//X버튼 비활성화
        save.deleteText[3].text = "";
        save.goSavePoint[3].SetActive(false);//저장소로 가는 버튼 비활성화
        save.saveButton[3].SetActive(true);//저장 버튼 활성화
        save.saveChapterName[3].text = "저장";//저장 만들기
        save.saveChapter[3].text = "";//입력된 값 삭제
        save.saveDate[3].text = "";
        PlayerPrefs.DeleteKey("save4");
        PlayerPrefs.DeleteKey("save4Name");
        PlayerPrefs.DeleteKey("save4Date");
    }
}
