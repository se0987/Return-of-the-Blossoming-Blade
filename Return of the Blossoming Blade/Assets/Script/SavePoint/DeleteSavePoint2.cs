using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeleteSavePoint2 : MonoBehaviour
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
        save.delete[1].SetActive(false);//X버튼 비활성화
        save.deleteText[1].text = "";
        save.goSavePoint[1].SetActive(false);//저장소로 가는 버튼 비활성화
        save.saveButton[1].SetActive(true);//저장 버튼 활성화
        save.saveChapterName[1].text = "저장";//저장 만들기
        save.saveChapter[1].text = "";//입력된 값 삭제
        save.saveDate[1].text = "";
        PlayerPrefs.DeleteKey("save2");
        PlayerPrefs.DeleteKey("save2Name");
        PlayerPrefs.DeleteKey("save2Date");
    }
}
