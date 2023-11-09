using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCutScene2 : MonoBehaviour
{
    public Dialogue dialogue_1;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public bool finish = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one && finish)
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

        //청명 HP 5% 이하, +100 이하
        //(청명 HP 5% 이상) (화산 상태 100%)(+250이상)
        //(동료 %>80% && 청명 HP <5%)(+250이상)
        //(동료 %>90% && 청명 HP <5% && 당보 생존)(+300이상)
        //(천마 못잡았을 때) (화산 상태 0%)
        //(+325)(대산혈사 후 청명 HP 90% 이상)

        theOrder.Move();
    }
}

