using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene7_1 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    public PlayerManager thePlayer;
    public GameObject forestPointDebug1;
    public GameObject forestPointDebug2;


    //private bool flag;
    private bool can = false;
    private bool one = true;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one)
        {
            one = false;
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        PlayerManager.instance.notMove = true;
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        PlayerManager.instance.notMove = false;
        forestPointDebug1.SetActive(false);
        forestPointDebug2.SetActive(false);
    }
}
