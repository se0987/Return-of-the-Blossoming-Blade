using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndingDialogueManager : MonoBehaviour
{
    public List<TextMeshProUGUI> ending;

    private string end1 = "1 동귀어진";
    private string end2 = "2 매화검존 청명 생존";
    private string end3 = "3 천려일득";
    private string end4 = "4 암존 당보 생존";
    private string end5 = "5 천마천세 만마앙복";
    private string end6 = "6 천하제일검문";

    public MainDialogue mainDialogue;

    // Start is called before the first frame update
    void Start()
    {
        mainDialogue.close.text = "";
        for (int i = 0; i < 6; i++)
        {
            ending[i].text = "";
            if (!PlayerPrefs.HasKey("End" + i.ToString()))
            {
                PlayerPrefs.SetInt("End" + i.ToString(), 0);
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
        mainDialogue.close.text = "닫기";
        if (PlayerPrefs.GetInt("End1") == 1)
        {
            ending[0].text = end1;
        }
        else
        {
            ending[0].text = "1";
        }

        if (PlayerPrefs.GetInt("End2") == 1)
        {
            ending[1].text = end2;
        }
        else
        {
            ending[1].text = "2";
        }

        if (PlayerPrefs.GetInt("End3") == 1)
        {
            ending[2].text = end3;
        }
        else
        {
            ending[2].text = "3";
        }

        if (PlayerPrefs.GetInt("End4") == 1)
        {
            ending[3].text = end4;
        }
        else
        {
            ending[3].text = "4";
        }

        if (PlayerPrefs.GetInt("End5") == 1)
        {
            ending[4].text = end5;
        }
        else
        {
            ending[4].text = "5";
        }

        if (PlayerPrefs.GetInt("End6") == 1)
        {
            ending[5].text = end6;
        }
        else
        {
            ending[5].text = "6";
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
        for (int i = 0; i < 6; i++)
        {
            ending[i].text = "";
        }
    }
}
