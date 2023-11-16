using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint3 : MonoBehaviour
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
        save.delete[2].SetActive(false);//X버튼 비활성화
        save.deleteText[2].text = "";
        save.goSavePoint[2].SetActive(false);//저장소로 가는 버튼 비활성화
        save.saveButton[2].SetActive(true);//저장 버튼 활성화
        save.saveChapterName[2].text = "저장";//저장 만들기
        save.saveChapter[2].text = "";//입력된 값 삭제
        save.saveDate[2].text = "";
        PlayerPrefs.DeleteKey("save3");
        PlayerPrefs.DeleteKey("save3Name");
        PlayerPrefs.DeleteKey("save3Date");
    }
}
