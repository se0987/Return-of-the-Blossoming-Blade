using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseContinue : MonoBehaviour
{

    private ContinueDialogueManager continueDialogueManager;

    void Start()
    {
        continueDialogueManager = FindObjectOfType<ContinueDialogueManager>();
    }

    // Start is called before the first frame update
    public void OnBtnClick()
    {
        continueDialogueManager.CloseDialogue();
    }
}
