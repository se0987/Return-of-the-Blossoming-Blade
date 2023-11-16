using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene4 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private DCutScene5 dCutScene5;

    //private bool flag;
    private bool can = false;

    private bool oneStart = true;
    private bool oneFace2 = true;
    private bool oneEnd = true;

    public bool start = false;
    public bool face2 = false;
    public bool end = false;
    
    public GameObject arrow1;

    public float time = 0f;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        dCutScene5 = FindObjectOfType<DCutScene5>();
        theDM.OnExitDialogue += HandleExitDialogue;
    }

    void HandleExitDialogue()
    {
        Debug.Log("ÁßÁö");
        stop = true;
        StopCoroutine(EventCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(oneStart && start)
        {
            oneStart = false;
            StartCoroutine(EventCoroutine());
        }
        else if (face2 && oneFace2)
        {
            oneFace2 = false;
            StartCoroutine(EventCoroutine());
        }else if (oneEnd && end)
        {
            oneEnd = false;
            StartCoroutine(EventCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        can = false;
    }

    IEnumerator EventCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        if (start)
        {
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(() => !theDM.talking);
            start = false;
        }
        else if (face2)
        {
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(() => !theDM.talking);
            face2 = false;
        }
        else if(end)
        {
            theDM.ShowDialogue(dialogue_3);
            yield return new WaitUntil(() => !theDM.talking);
            end = false;
            PlayerPrefs.SetFloat("ChunSalTime", time);
            if(time > 10)
            {
                theOrder.Action("DangBo", "DIE");
                dCutScene5.first = false;
            }
            dCutScene5.enable = true;
            arrow1.SetActive(true);
        }
        theOrder.Move();
    }
}
