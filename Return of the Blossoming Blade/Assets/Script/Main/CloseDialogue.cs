using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogue : MonoBehaviour
{

    private ContinueDialogueManager continueDialogueManager;
    private EndingDialogueManager endingDialogueManager;

    void Start()
    {
        continueDialogueManager = FindObjectOfType<ContinueDialogueManager>();
        endingDialogueManager = FindObjectOfType<EndingDialogueManager>();
    }

    // Start is called before the first frame update
    public void OnBtnClick()
    {
        continueDialogueManager.CloseDialogue();
        endingDialogueManager.CloseDialogue();
    }
}
