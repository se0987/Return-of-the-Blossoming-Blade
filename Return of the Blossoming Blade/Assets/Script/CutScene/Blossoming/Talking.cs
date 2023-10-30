using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;
    private DialogueManager theDM;

    private bool can = false;
    private bool one = true;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        can = true;
/*        if (can)
        {
            if (collision.gameObject.name == "Player")
            {
                theDM.ShowDialogue(dialogue);
            }
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        can = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (one && can && Input.GetKeyDown(KeyCode.C))
        {
            one = false;
            theDM.ShowDialogue(dialogue);
        }
    }
}
