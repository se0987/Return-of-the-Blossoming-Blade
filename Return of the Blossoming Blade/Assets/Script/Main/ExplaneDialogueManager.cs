using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExplaneDialogueManager : MonoBehaviour
{
    public MainDialogue mainDialogue;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        mainDialogue.close.text = "";
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
        anim.SetBool("Appear", true);
    }

    public void CloseDialogue()
    {
        mainDialogue.close.text = "";
        anim.SetBool("Appear", false);
        mainDialogue.anim.SetBool("Appear", false);
        mainDialogue.btn1.interactable = true;
        mainDialogue.btn2.interactable = true;
        mainDialogue.btn3.interactable = true;
        mainDialogue.btn4.interactable = true;
    }
}
