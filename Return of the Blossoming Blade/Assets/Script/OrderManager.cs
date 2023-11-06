using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private PlayerManager thePlayer; // 이벤트 도중 키 입력 방지
    private List<MovingObject> characters;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    public void PreLoadCharacter()
    {
        characters = ToList();
    }

    public List<MovingObject> ToList()
    {
        List<MovingObject> tempList = new List<MovingObject>();
        MovingObject[] temp = FindObjectsOfType<MovingObject>();

        for(int i=0; i<temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }

        return tempList;
    }

    public void NotMove()
    {
        thePlayer.notMove = true;
    }

    public void Move()
    {
        thePlayer.notMove = false;
    }

    public void Move(string _name, string _dir, string _state = "Walking")
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].Move(_dir, 5, _state);
            }
        }
    }

    public void Turn(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirY", 0f);
                characters[i].animator.SetFloat("DirX", 0f);
                switch (_dir)
                {
                    case "UP":
                        characters[i].animator.SetFloat("DirY", 1f);
                        break;
                    case "DOWN":
                        characters[i].animator.SetFloat("DirY", -1f);
                        break;
                    case "RIGHT":
                        characters[i].animator.SetFloat("DirX", 1f);
                        break;
                    case "LEFT":
                        characters[i].animator.SetFloat("DirX", -1f);
                        break;
                }
            }
        }
    }

    public void Appear(string _name, bool _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetBool("Appear", _dir);
                characters[i].boxCollider.enabled = _dir;
            }
        }
    }

    public void Action(string _name, string _dir)
    {//추후 청명이 액션 명령 넣기
        StartCoroutine(StartChapterCoroutine(_name, _dir));
    }

    IEnumerator StartChapterCoroutine(string _name, string _dir)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (_name == characters[i].characterName)
            {
                characters[i].animator.SetFloat("DirY", 0f);
                characters[i].animator.SetFloat("DirX", 0f);
                switch (_dir)
                {
                    case "AttackH"://청명 액션
                        characters[i].animator.SetBool("AttackH", true);
                        yield return new WaitForSeconds(1f);
                        characters[i].animator.SetBool("AttackH", false);
                        break;
                    case "DIE":
                        characters[i].animator.SetBool("Die", true);
                        break;
                    case "NOTDIE":
                        characters[i].animator.SetBool("Die", false);
                        break;
                    case "WAKEUP":
                        characters[i].animator.SetBool("WakeUp", true);
                        break;
                    case "NOTWAKEUP":
                        characters[i].animator.SetBool("WakeUp", false);
                        break;
                    case "LAST":
                        characters[i].animator.SetBool("Last", true);
                        characters[i].speed = 1;
                        characters[i].walkCount = 20;
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
