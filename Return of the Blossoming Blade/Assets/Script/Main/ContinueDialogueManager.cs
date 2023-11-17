using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContinueDialogueManager : MonoBehaviour
{
    public List<TextMeshProUGUI> saveChapter;
    public List<TextMeshProUGUI> saveChapterName;
    public List<TextMeshProUGUI> saveDate;
    public List<GameObject> goSavePoint;

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
            goSavePoint[i].SetActive(false);
            if (!PlayerPrefs.HasKey("save" + i.ToString()))
            {
                PlayerPrefs.SetString("save" + i.ToString(), "");
            }
        }
    }

    public void ShowDialogue()
    {
        StartCoroutine(ShowCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        mainDialogue.anim.SetBool("Appear", true);
        mainDialogue.btn1.interactable = false;
        mainDialogue.btn2.interactable = false;
        mainDialogue.btn3.interactable = false;
        mainDialogue.btn4.interactable = false;
        yield return new WaitForSeconds(0.5f);
        mainDialogue.close.text = "´Ý±â";
        for (int i = 0; i < 5; i++)
        {
            saveChapter[i].text = PlayerPrefs.GetString("save" + (i + 1).ToString());
            saveChapterName[i].text = PlayerPrefs.GetString("save" + (i + 1).ToString() + "Name");
            saveDate[i].text = PlayerPrefs.GetString("save" + (i + 1).ToString() + "Date");
            if (saveChapter[i].text != "")
            {
                goSavePoint[i].SetActive(true);
            }
        }
    }

    public void CloseDialogue()
    {
        mainDialogue.close.text = "";
        mainDialogue.anim.SetBool("Appear", false);
        mainDialogue.btn1.interactable = true;
        mainDialogue.btn2.interactable = true;
        mainDialogue.btn3.interactable = true;
        mainDialogue.btn4.interactable = true;
        for (int i = 0; i < 5; i++)
        {
            saveChapter[i].text = "";
            saveChapterName[i].text = "";
            saveDate[i].text = "";
            goSavePoint[i].SetActive(false);
        }
    }
}
