using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI name;
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogueWindow;

    private List<string> listSentences;
    private List<string> listName;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;

    private int count;//대화 진행 상황

    public Animator animSprite;
    public Animator animDialogueWindow;

    public string typeSound;
    public string CSound;

    private AudioManager theAudio;

    public bool talking = false;
    private bool keyActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        text.text = "";
        name.text = "";
        listSentences = new List<string>();
        listName = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        theAudio = FindObjectOfType<AudioManager>();
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        talking = true;
        for(int i=0; i<dialogue.sentences.Length; i++)
        {
            listSentences.Add(dialogue.sentences[i]);
            listName.Add(dialogue.names[i]);
            listSprites.Add(dialogue.sprites[i]);
            listDialogueWindows.Add(dialogue.dialogueWindows[i]);
        }
        animSprite.SetBool("Appear", true);
        animDialogueWindow.SetBool("Appear", true);
        StartCoroutine(StartDialogueCoroutine());
    }

    public void ExitDialogue()
    {
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);
        listSentences.Clear();
        listName.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        count = 0;
        text.text = "";
        name.text = "";
        talking = false;
    }

    IEnumerator StartDialogueCoroutine()
    {
        if (count > 0)
        {
            if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                yield return new WaitForSeconds(0.2f);
                animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);
                rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                animDialogueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            else
            {
                if (listSprites[count] != listSprites[count - 1])
                {
                    yield return new WaitForSeconds(0.1f);
                    animSprite.SetBool("Change", true);
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                    animSprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else
        {
            rendererDialogueWindow.GetComponent<SpriteRenderer>().sprite = listDialogueWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }
        name.text = listName[count];
        keyActivated = true;
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i];//1글자씩 출력
            if(i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                keyActivated = false;
                count++;
                text.text = "";
                name.text = "";
                theAudio.Play(CSound);
                if (count == listSentences.Count)
                {
                    StopAllCoroutines();
                    ExitDialogue();
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(StartDialogueCoroutine());
                }
            }
        }
    }
}
