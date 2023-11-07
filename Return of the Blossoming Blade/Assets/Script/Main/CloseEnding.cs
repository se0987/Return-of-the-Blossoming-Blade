using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEnding : MonoBehaviour
{

    private EndingDialogueManager endingDialogueManager;

    void Start()
    {
        endingDialogueManager = FindObjectOfType<EndingDialogueManager>();
    }

    // Start is called before the first frame update
    public void OnBtnClick()
    {
        endingDialogueManager.CloseDialogue();
    }
}
