using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCutScene3 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    // 보스 체력바
    public GameObject chunSal;
    public GameObject hpGauge;
    public GameObject bossName;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private BGMManager theBGM;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theBGM = FindObjectOfType<BGMManager>();
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
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");
        theOrder.Move("Player", "UP");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        theBGM.Play(2);

        // 보스 체력바
        if (chunSal != null)
        {
            chunSal.SetActive(true);
        }
        if (hpGauge != null)
        {
            hpGauge.SetActive(true);
        }
        if (bossName != null)
        {
            bossName.SetActive(true);
        }

        theOrder.Move();
    }
}
