using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveContinueDialogueManager : MonoBehaviour
{
    public List<TextMeshProUGUI> saveChapter;
    public List<TextMeshProUGUI> saveChapterName;
    public List<TextMeshProUGUI> saveDate;
    public List<GameObject> saveButton;
    public List<GameObject> goSavePoint;
    public List<GameObject> delete;
    public List<TextMeshProUGUI> deleteText;

    public MainDialogue mainDialogue;

    // Start is called before the first frame update
    void Start()
    {
        mainDialogue.close.text = "";
        for(int i=0; i<5; i++)
        {
            saveChapter[i].text = "";
            saveChapterName[i].text = "";
            saveDate[i].text = "";
            deleteText[i].text = "";
            saveButton[i].SetActive(false);
            goSavePoint[i].SetActive(false);
            delete[i].SetActive(false);
        }
    }

    public void ShowSaveDialogue()
    {
        StartCoroutine(ShowSaveCoroutine());
    }

    IEnumerator ShowSaveCoroutine()
    {
        mainDialogue.anim.SetBool("Appear", true);
        yield return new WaitForSeconds(0.5f);
        mainDialogue.close.text = "´Ý±â";
        for (int i = 0; i < 5; i++)
        {
            saveChapter[i].text = PlayerPrefs.GetString("save"+(i+1).ToString());
            saveChapterName[i].text = PlayerPrefs.GetString("save"+ (i + 1).ToString()+"Name");
            saveDate[i].text = PlayerPrefs.GetString("save"+ (i + 1).ToString()+"Date");
            if(saveChapter[i].text == "")
            {
                saveButton[i].SetActive(true);
                saveChapterName[i].text = "ÀúÀå";
            }
            else
            {
                goSavePoint[i].SetActive(true);
                delete[i].SetActive(true);
                deleteText[i].text = "X";
            }
        }
    }

    public void CloseSaveDialogue()
    {
        mainDialogue.close.text = "";
        mainDialogue.anim.SetBool("Appear", false);
        for (int i = 0; i < 5; i++)
        {
            goSavePoint[i].SetActive(false);
            saveButton[i].SetActive(false);
            delete[i].SetActive(false);
            saveChapter[i].text = "";
            saveChapterName[i].text = "";
            saveDate[i].text = "";
            deleteText[i].text = "";
        }
    }
}
