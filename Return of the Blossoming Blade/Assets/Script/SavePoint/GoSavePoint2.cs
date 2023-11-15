using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GoSavePoint2 : MonoBehaviour
{
    private SaveContinueDialogueManager save;
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<SaveContinueDialogueManager>();
        player = FindObjectOfType<PlayerManager>();
    }

    public void OnBtnClick()
    {
        //√¢ ¥›±‚
        player.allStop = false;
        player.notMove = false;
        save.CloseSaveDialogue();

        //∏  ¿Ãµø
        SceneManager.LoadScene(PlayerPrefs.GetString("save2SceneName"));
        //√ª∏Ì HP, MP
        PlayerPrefs.SetFloat("playerHP", PlayerPrefs.GetFloat("save2PlayerHP"));
        PlayerPrefs.SetFloat("playerMP", PlayerPrefs.GetFloat("save2PlayerMP"));
        //øµæ‡
        PlayerPrefs.SetFloat("havePosion", PlayerPrefs.GetFloat("save2HavePosion"));
        PlayerPrefs.SetFloat("maxPosion", PlayerPrefs.GetFloat("save2MaxPosion"));

    }
}
