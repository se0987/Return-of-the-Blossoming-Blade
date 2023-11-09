using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUser : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    private bool isHorizontalPressed = false;
    private bool isVerticalPressed = false;

    void Update()
    {
        CheckKeyPress();
        MoveCharacter();
    }

    void CheckKeyPress()
    {
        // 방향키가 눌려있지 않을 때만 새로운 키 입력 가능
        if (!isHorizontalPressed && !isVerticalPressed)
        {
            isHorizontalPressed = Input.GetAxisRaw("Horizontal") != 0;
            isVerticalPressed = Input.GetAxisRaw("Vertical") != 0;
        }
        else
        {
            if (isHorizontalPressed && Input.GetAxisRaw("Horizontal") == 0)
            {
                isHorizontalPressed = false;
            }
            if (isVerticalPressed && Input.GetAxisRaw("Vertical") == 0)
            {
                isVerticalPressed = false;
            }
        }
    }

    void MoveCharacter()
    {
        float horizontal = 0;
        float vertical = 0;

        if (isHorizontalPressed)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else if (isVerticalPressed)
        {
            vertical = Input.GetAxisRaw("Vertical");
        }

        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

}

