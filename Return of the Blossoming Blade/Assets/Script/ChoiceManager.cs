using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public TextMeshProUGUI choice1;
    public TextMeshProUGUI choice2;
    public SpriteRenderer rendererDialogueWindow;
    public SpriteRenderer rendererFlower1;
    public SpriteRenderer rendererFlower2;

    private string choice_1;
    private string choice_2;

    public Animator animDialogueWindow;
    public Animator animFlower1;
    public Animator animFlower2;

    public bool talking = false;
    private bool keyActivated = false;

    private AudioManager theAudio;

    private int nowChoice= 1;

    private int saveN = 0;

    public string clickSound;

    // Start is called before the first frame update
    void Start()
    {
        choice1.text = "";
        choice2.text = "";
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowChoice(Choice choice, int n)
    {
        saveN = n;
        talking = true;
        choice_1 = choice.answer1;
        choice1.text = "<color=#ef538b>" + choice_1 + "</color>";
        choice_2 = choice.answer2;
        choice2.text = choice_2;
        animDialogueWindow.SetBool("Appear", true);
        animFlower1.SetBool("Choice", true);
        animFlower2.SetBool("Choice", false);
        rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = choice.dialogueWindows;
        keyActivated = true;
    }

    public void ExitChoice()
    {
        animDialogueWindow.SetBool("Appear", false);
        talking = false;
        choice1.text = "";
        choice2.text = "";
        animFlower1.SetBool("Choice", false);
        animFlower2.SetBool("Choice", false);
        PlayerPrefs.SetInt("choice" + saveN.ToString(), nowChoice);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(nowChoice != 1)
                {
                    theAudio.Play(clickSound);
                    nowChoice = 1;

                    choice1.text = "<color=#ef538b>" + choice_1 + "</color>";
                    choice2.text = choice_2;

                    animFlower1.SetBool("Choice", true);
                    animFlower2.SetBool("Choice", false);
                }
            }else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (nowChoice == 1)
                {
                    theAudio.Play(clickSound);
                    nowChoice = 2;

                    choice1.text = choice_1;
                    choice2.text = "<color=#ef538b>" + choice_2 + "</color>";

                    animFlower1.SetBool("Choice", false);
                    animFlower2.SetBool("Choice", true);
                }
            }else if (Input.GetKeyDown(KeyCode.C))
            {
                ExitChoice();
            }
        }
    }
}
