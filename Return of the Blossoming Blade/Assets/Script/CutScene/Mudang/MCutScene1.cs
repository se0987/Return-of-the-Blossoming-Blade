using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCutScene1 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;

    public CameraManager theCamera; // 카메라 매니저 참조 추가


    //private bool flag;
    private bool can = false;
    private bool one = true;
    private bool stop = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        theDM.OnExitDialogue += HandleExitDialogue;
    }

    void HandleExitDialogue()
    {
        Debug.Log("중지");
        stop = true;
        StopCoroutine(EventCoroutine());
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
        theCamera.SetCutsceneMode(true, new Vector3(-1384f, -276f, theCamera.transform.position.z)); // 컷신 위치로 이동

        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
        if (stop)
        {
            yield break;
        }
        theOrder.Action("Student1", "DIE");

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Move();
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");
        theOrder.Move("DangBo", "LEFT");

        theCamera.SetCutsceneMode(false, Vector3.zero); // 컷신 종료 후 원래 모드로 돌아가기
    }
}
