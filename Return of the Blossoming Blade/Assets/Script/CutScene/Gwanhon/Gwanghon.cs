using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gwanghon : MonoBehaviour
{
    public Transform playerTransform;

    public float movementSpeed = 3.0f;

    private bool isAttacking = false;
    private bool isTeleporting = false;

    // 종류에 관계없이, 모든 공격 사이에 적용되는 1초의 쿨타임
    private float globalLastAttackTime = 0f;
    private float globalCooldown = 1f;

    [System.Serializable]
    public class AttackData
    {
        public float range = 50.0f;
        public float cooldownTime = 2.0f;
        public float durationTime = 0.8f;
        public float nextAttackTime = 0f;
        public float moveDistance = 0f;
        public float afterAttackDuration = 0f;
    }

    // 공격별 데이터
    public AttackData slashAttack;
    public AttackData swingAttack;
    public AttackData dashAttack;
    public AttackData teleportAttack;

    Vector2 movement = new Vector2();
    Rigidbody2D rigidbody2D;

    Animator animator;
    string animationsState = "DirectionState";

    enum States
    {
        right = 1,
        front = 2,
        left = 3,
        back = 4,

        moveRight = 5,
        moveFront = 6,
        moveLeft = 7,
        moveBack = 8,

        slashRight = 9,
        slashFront = 10,
        slashLeft = 11,
        slashBack = 12,

        swingRight = 13,
        swingFront = 14,
        swingLeft = 15,
        swingBack = 16,

        dashRight = 17,
        dashFront = 18,
        dashLeft = 19,
        dashBack = 20,

        startTeleportRight = 21,
        startTeleportFront = 22,
        startTeleportLeft = 23,
        startTeleportBack = 24,

        endTeleportRight = 25,
        endTeleportFront = 26,
        endTeleportLeft = 27,
        endTeleportBack = 28
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAttacking || isTeleporting) return;

        Vector2 direction = playerTransform.position - transform.position;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float currentTime = Time.time;

        if (AllAttacksOnCooldown() || distance <= (slashAttack.range + 3f) && currentTime <= globalLastAttackTime + globalCooldown)
        {
            Idle(direction);
            return;
        }
        else
        {
            if (distance <= slashAttack.range && Time.time >= slashAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Slash(direction);
                slashAttack.nextAttackTime = Time.time + slashAttack.cooldownTime;
                globalLastAttackTime = currentTime + slashAttack.durationTime;
            }
            else if (distance <= swingAttack.range && Time.time >= swingAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Swing(direction);
                swingAttack.nextAttackTime = Time.time + swingAttack.cooldownTime;
                globalLastAttackTime = currentTime + swingAttack.durationTime;
            }
            else if (distance <= dashAttack.range && Time.time >= dashAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Dash(direction);
                dashAttack.nextAttackTime = Time.time + dashAttack.cooldownTime;
                globalLastAttackTime = currentTime + dashAttack.durationTime;
            }
            else if (distance <= teleportAttack.range && Time.time >= teleportAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Teleport(direction);
                teleportAttack.nextAttackTime = Time.time + teleportAttack.cooldownTime + teleportAttack.afterAttackDuration + 5.0f;
                globalLastAttackTime = currentTime + teleportAttack.durationTime;
                StartCoroutine(TeleportMovement(direction));
            }
            else
            {
                if (!isAttacking)
                {
                    FollowPlayer(direction);
                }
            }
        }
    }

    bool AllAttacksOnCooldown()
    {
        float currentTime = Time.time;
        return currentTime < slashAttack.nextAttackTime &&
               currentTime < swingAttack.nextAttackTime &&
               currentTime < dashAttack.nextAttackTime &&
               currentTime < teleportAttack.nextAttackTime;
    }

    private IEnumerator DashMovement(Vector2 direction)
    {
        isAttacking = true;

        rigidbody2D.velocity = Vector2.zero;
        Vector2 dashStartPos = this.transform.position;

        yield return new WaitForSeconds(dashAttack.durationTime);

        Vector2 dashEndPos = dashStartPos + direction * dashAttack.moveDistance;
        rigidbody2D.MovePosition(dashEndPos);

        rigidbody2D.velocity = Vector2.zero;

        yield return new WaitForSeconds(dashAttack.afterAttackDuration);

        isAttacking = false;
    }

    private void LookAtPlayer(Vector2 lookDirection)
    {
        if (Mathf.Abs(lookDirection.x) > Mathf.Abs(lookDirection.y))
        {
            if (lookDirection.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.endTeleportRight);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.endTeleportLeft);
            }
        }
        else
        {
            if (lookDirection.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.endTeleportBack);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.endTeleportFront);
            }
        }
    }

    private IEnumerator TeleportMovement(Vector2 direction)
    {
        isAttacking = true;
        isTeleporting = true;

        yield return new WaitForSeconds(teleportAttack.durationTime);

        GetComponent<SpriteRenderer>().enabled = false;

        float waitTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(waitTime);

        // 플레이어가 바라보는 방향의 반대를 구합니다.
        Vector2 playerForward = playerTransform.TransformDirection(Vector2.up);
        Vector2 teleportPosition = (Vector2)playerTransform.position + (playerForward * teleportAttack.moveDistance); // 반대 방향으로 바꿉니다.

        transform.position = teleportPosition;

        GetComponent<SpriteRenderer>().enabled = true;

        // 플레이어의 방향을 바라보도록 보스의 방향 설정
        Vector2 lookDirection = playerTransform.position - transform.position;
        LookAtPlayer(lookDirection); // 바라보는 방향의 반대쪽으로 설정

        yield return new WaitForSeconds(teleportAttack.afterAttackDuration);

        isTeleporting = false;
    }

    void Idle(Vector2 direction)
    {
        // IDLE 상태 유지하는 동안은 움직이지 않도록
        rigidbody2D.velocity = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.right);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.left);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.back);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.front);
            }
        }
    }

    void FollowPlayer(Vector2 direction)
    {
        // 움직임을 상하좌우로만 제한
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
        }
        else
        {
            direction.x = 0;
        }

        direction.Normalize();

        // 적을 결정한 방향으로 이동
        rigidbody2D.velocity = direction * movementSpeed;
        
        UpdateState(direction);
    }

    void Slash(Vector2 direction)
    {
        isAttacking = true;

        // 공격 중에는 움직이지 않음
        rigidbody2D.velocity = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.slashRight);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.slashLeft);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.slashBack);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.slashFront);
            }
        }

        // 공격 애니메이션 후 isAttacking 다시 false로
        StartCoroutine(AttackCooldown(slashAttack.durationTime));

        // 적중 시 플레이어 HP 감소 등 추가 로직 여기에 구현
    }

    void Swing(Vector2 direction)
    {
        isAttacking = true;

        // 공격 중에는 움직이지 않음
        rigidbody2D.velocity = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.swingRight);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.swingLeft);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.swingBack);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.swingFront);
            }
        }

        // 공격 애니메이션 후 isAttacking 다시 false로
        StartCoroutine(AttackCooldown(swingAttack.durationTime));

        // 적중 시 플레이어 HP 감소 등 추가 로직 여기에 구현
    }

    void Dash(Vector2 direction)
    {
        isAttacking = true;

        // 공격 중에는 움직이지 않음
        rigidbody2D.velocity = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.dashRight);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.dashLeft);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.dashBack);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.dashFront);
            }
        }

        // 공격 애니메이션 후 isAttacking 다시 false로
        StartCoroutine(AttackCooldown(dashAttack.durationTime));
        StartCoroutine(DashMovement(direction));

        // 적중 시 플레이어 HP 감소 등 추가 로직 여기에 구현
    }

    void Teleport(Vector2 direction)
    {
        isAttacking = true;

        // 공격 중에는 움직이지 않음
        rigidbody2D.velocity = Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.startTeleportRight);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.startTeleportLeft);
            }
        }
        {
            if (direction.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.startTeleportBack);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.startTeleportFront);
            }
        }

        // 공격 애니메이션 후 isAttacking 다시 false로
        StartCoroutine(AttackCooldown(teleportAttack.durationTime));

        // 적중 시 플레이어 HP 감소 등 추가 로직 여기에 구현
    }

    private IEnumerator AttackCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    private void UpdateState(Vector2 direction)
    {
        if (direction.x > 0.1f)
        {
            animator.SetInteger(animationsState, (int)States.moveRight);
        }

        else if (direction.x < -0.1f)
        {
            animator.SetInteger(animationsState, (int)States.moveLeft);
        }

        else if (direction.y > 0.1f)
        {
            animator.SetInteger(animationsState, (int)States.moveBack);
        }

        else if (direction.y < -0.1f)
        {
            animator.SetInteger(animationsState, (int)States.moveFront);
        }
    }
}
