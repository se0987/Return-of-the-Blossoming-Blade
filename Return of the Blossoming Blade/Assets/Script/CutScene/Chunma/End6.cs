using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End6 : MonoBehaviour
{
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    public int playMusicTrack1;
    public int playMusicTrack2;

    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private ChoiceManager theChoice;
    private ChapterManager theChapter;

    private BGMManager bgmManager;
    public string swordSound;
    private AudioManager theAudio;

    //private bool flag;
    private bool can = false;
    private bool one = true;

    public static bool end = false;

    public GameObject endingIllustration; // 엔딩 일러스트 GameObject
    public GameObject posionManagerOff;

    public GameObject hpBar;
    public GameObject bossName;

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
                if (temp[i].gateName.Equals("EndPoint6"))
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
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        yield return new WaitForSeconds(0.2f);
        bgmManager.Stop();
        bgmManager.Play(playMusicTrack1);

        theOrder.Action("Player", "LAST");


        hpBar.SetActive(false);
        bossName.SetActive(false);

        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDM.talking);
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
        theOrder.Action("Player", "AttackH");

        GameObject Cheonma = GameObject.Find("Cheonma Bon In");
        GameObject bossHpBarObject = GameObject.Find("Boss_HP_Gauge1");
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
        bgmManager.Stop();
        theAudio.Play(swordSound);
        bgmManager.Play(playMusicTrack2);

        theDM.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDM.talking);

        theOrder.Appear("BlackScreen", true);
        posionManagerOff.SetActive(false);
        // 일러스트를 서서히 나타나게 하는 Coroutine 호출
        StartCoroutine(ShowEndingIllustration());
        theDM.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => !theDM.talking);

        theChapter.ShowChapter("결말 6\n천하제일검문");
        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene("Main");

        theOrder.Move();
    }
    IEnumerator ShowEndingIllustration()
    {
        Image illustration = endingIllustration.GetComponent<Image>();
        float duration = 2.0f; // 페이드 인 지속 시간
        float elapsed = 0f;

        endingIllustration.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);
            illustration.color = new Color(illustration.color.r, illustration.color.g, illustration.color.b, alpha);
            yield return null;
        }
    }
}

