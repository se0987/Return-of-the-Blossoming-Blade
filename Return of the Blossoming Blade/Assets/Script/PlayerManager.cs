using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    public static PlayerManager instance;

    public string currentMapName;

    private float applyRunSpeed;

    private bool canMove = true;

    public bool notMove = false;

    public bool skillNotMove = false;

    private PlayerStatus playerStatus;

    public string walkSound;
    private AudioManager theAudio;
    private SaveContinueDialogueManager theContinue;
    public bool allStop = false;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<string>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        theAudio = FindObjectOfType<AudioManager>();
        theContinue = FindObjectOfType<SaveContinueDialogueManager>();
    }

    IEnumerator MoveCoroutine()
    {
        while ((Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && !notMove && !skillNotMove)
        {
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            bool checkCollsionFlag = base.CheckCollsion();
            if (checkCollsionFlag)
            {
                break;
            }

            animator.SetBool("Running", true);
            theAudio.Play(walkSound);

            //sboxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * speed, 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * speed, 0);
                }
                currentWalkCount++;
                if (currentWalkCount == 12)
                {
                    boxCollider.offset = Vector2.zero;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Running", false);
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allStop)
        {
            if (canMove && !notMove && !skillNotMove)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    canMove = false;
                    StartCoroutine(MoveCoroutine());
                }
            }
            if (!notMove && !skillNotMove)
            {
                if (Input.GetKeyDown(KeyCode.F))//포션
                {
                    playerStatus.UsePosion();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))//세이브
            {
                notMove = true;
                allStop = true;
                theContinue.ShowSaveDialogue();
            }
        }
    }

    public void MovePlayer(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

}
