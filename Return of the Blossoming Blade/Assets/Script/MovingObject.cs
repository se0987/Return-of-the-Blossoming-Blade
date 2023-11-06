using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;

    public float speed;
    public string characterName;

    protected Vector3 vector;

    public int walkCount;
    protected int currentWalkCount;
    public Animator animator;

    private bool notCoroutine = false;
    public Queue<string> queue;

    public void Move(string _dir, int _frequency = 5, string _state = "Walking")
    {
        queue.Enqueue(_dir);
        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency, _state));
        }
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency, string _state)
    {
        while (queue.Count != 0)
        {
            switch (_frequency)
            {
                case 1:
                    yield return new WaitForSeconds(4f);
                    break;
                case 2:
                    yield return new WaitForSeconds(3f);
                    break;
                case 3:
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    yield return new WaitForSeconds(1f);
                    break;
                case 5:
                    break;
            }

            string direction = queue.Dequeue();
            vector.Set(0, 0, vector.z);

            switch (direction)
            {
                case "UP":
                    vector.y = 1f;
                    break;
                case "DOWN":
                    vector.y = -1f;
                    break;
                case "RIGHT":
                    vector.x = 1f;
                    break;
                case "LEFT":
                    vector.x = -1f;
                    break;
                case "TURNLEFT":
                    animator.SetBool("Turn", true);
                    break;
                case "TURNFRONT":
                    animator.SetBool("Turn", false);
                    break;

            }

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            while (true)
            {
                bool checkCollsionFlag = CheckCollsion();
                if (checkCollsionFlag)
                {
                    animator.SetBool(_state, false);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break;
                }
            }

            animator.SetBool(_state, true);

            while (currentWalkCount < walkCount)
            {
                transform.Translate(vector.x * speed, vector.y * speed, 0);
                currentWalkCount++;
                if(currentWalkCount == 12)
                {
                    boxCollider.offset = Vector2.zero;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
            if (_frequency != 5)
                animator.SetBool(_state, false);
        }
        animator.SetBool(_state, false);
        notCoroutine = false;
    }

    protected bool CheckCollsion()
    {
        RaycastHit2D hit;

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
