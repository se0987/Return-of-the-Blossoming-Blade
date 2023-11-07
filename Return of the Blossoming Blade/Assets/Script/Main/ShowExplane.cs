using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowExplane : MonoBehaviour
{

    private ExplaneDialogueManager explaneDialogueManager;

    void Start()
    {
        explaneDialogueManager = FindObjectOfType<ExplaneDialogueManager>();
    }

    // Start is called before the first frame update
    public void OnBtnClick()
    {
        explaneDialogueManager.ShowDialogue();
    }
}
