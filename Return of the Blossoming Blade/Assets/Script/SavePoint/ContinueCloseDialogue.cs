using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueCloseDialogue : MonoBehaviour
{
    private SaveContinueDialogueManager continueDialogueManager;
    private PlayerManager player;
    // Start is called before the first frame update
    void Start()
    {
        continueDialogueManager = FindObjectOfType<SaveContinueDialogueManager>();
        player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    public void OnBtnClick()
    {
        player.allStop = false;
        player.notMove = false;
        continueDialogueManager.CloseSaveDialogue();
    }
}
