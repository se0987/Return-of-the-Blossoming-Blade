using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End2 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;
    public Dialogue dialogue_4;
    public Dialogue dialogue_5;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;

    public int playMusicTrack1;
    public int playMusicTrack2;
    private BGMManager bgmManager;
    public string swordSound;
    private AudioManager theAudio;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public static bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
        bgmManager = FindObjectOfType<BGMManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theChapter = FindObjectOfType<ChapterManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (one && end)
        {
            one = false;
            TransferMap[] temp = FindObjectsOfType<TransferMap>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].gateName.Equals("EndPoint2"))
                {
                    temp[i].move = false;
                    break;
                }
            }
            StartCoroutine(EventCoroutine());
        }
    }

    IEnumerator EventCoroutine()
    {
        GameObject Cheonma = GameObject.Find("Cheonma Bon In");
        GameObject bossHpBarObject = GameObject.Find("Boss_HP_Gauge1");
        GameObject bossName = GameObject.Find("BossName");

        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);

        bgmManager.Stop();
        bgmManager.Play(playMusicTrack1);

        theOrder.Action("Player", "LAST");

        bossHpBarObject.SetActive(false);
        bossName.SetActive(false);
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);

        theDM.ShowDialogue(dialogue_2);
        bossHpBarObject.SetActive(true);
        bossName.SetActive(true);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitForSeconds(0.2f);
        theOrder.Move("Player", "UP");
        yield return new WaitUntil(() => !theDM.talking);
        bossHpBarObject.SetActive(false);
        bossName.SetActive(false);

        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);


        if (bossHpBarObject != null)
        {
            Image bossHpBarImage = bossHpBarObject.GetComponent<Image>();
            if (bossHpBarImage != null)
            {
                bossHpBarImage.fillAmount = 0.01f;
            }
        }

        theOrder.Action("Player", "AttackH");
        StartCoroutine(SetFillAmountToZeroAfterDelay(2f));


        if (bossHpBarObject != null)
        {
            Image bossHpBarImage = bossHpBarObject.GetComponent<Image>();
            if (bossHpBarImage != null)
            {
                bossHpBarImage.fillAmount = 0;
            }
        }
        if (Cheonma != null)
        {
            Cheonma.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        bgmManager.Stop();
        theAudio.Play(swordSound);
        bgmManager.Play(playMusicTrack2);

        theDM.ShowDialogue(dialogue_4);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Appear("BlackScreen", true);
        theDM.ShowDialogue(dialogue_5);
        yield return new WaitUntil(() => !theDM.talking);

        theChapter.ShowChapter("결말 2\n매화검존 청명 생존");
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Main");

        theOrder.Move();
    }

    IEnumerator SetFillAmountToZeroAfterDelay(float delay)
    {
        GameObject Cheonma = GameObject.Find("Cheonma Bon In");
        GameObject bossHpBarObject = GameObject.Find("Boss_HP_Gauge1");
        GameObject bossName = GameObject.Find("BossName");

        // 지연 시간 대기
        yield return new WaitForSeconds(delay);

        // 지연 시간 후에 fillAmount를 0으로 설정
        if (bossHpBarObject != null)
        {
            Image bossHpBarImage = bossHpBarObject.GetComponent<Image>();
            if (bossHpBarImage != null)
            {
                bossHpBarImage.fillAmount = 0;
            }
        }

        // 다른 작업 수행 (예: Cheonma 비활성화)
        if (Cheonma != null)
        {
            Cheonma.SetActive(false);
        }
    }
}

