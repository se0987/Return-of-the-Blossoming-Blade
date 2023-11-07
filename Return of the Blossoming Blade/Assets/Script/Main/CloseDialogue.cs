using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDialogue : MonoBehaviour
{

    private ContinueDialogueManager continueDialogueManager;
    private EndingDialogueManager endingDialogueManager;
    private ExplaneDialogueManager explaneDialogueManager;

    void Start()
    {
        continueDialogueManager = FindObjectOfType<ContinueDialogueManager>();
        endingDialogueManager = FindObjectOfType<EndingDialogueManager>();
        explaneDialogueManager = FindObjectOfType<ExplaneDialogueManager>();
    }

    // Start is called before the first frame update
    public void OnBtnClick()
    {
        continueDialogueManager.CloseDialogue();
        endingDialogueManager.CloseDialogue();
        explaneDialogueManager.CloseDialogue();
    }
}
