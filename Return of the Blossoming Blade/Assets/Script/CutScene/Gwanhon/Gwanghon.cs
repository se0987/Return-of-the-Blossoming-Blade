using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// fasdfsda
public class Gwanghon : MonoBehaviour
{
    public Transform playerTransform;

    public float movementSpeed = 3.0f;

    private bool isAttacking = false;
    private bool isTeleporting = false;

    private float globalLastAttackTime = 0f;
    private float globalCooldown = 1f;

    public GameObject attackRangeIndicator;
    private SpriteRenderer rangeRenderer;
    public AttackData currentAttack = null;
    public Transform bossTransform;
    public Sprite rectangleRangeSprite;
    private AudioManager theAudio;
    private SpriteRenderer spriteRenderer;
    public float flashDuration = 0.2f;
    private Color originalColor;

    [System.Serializable]
    public class AttackData
    {
        public float range = 50.0f;
        public float minRange = 0f;
        public float cooldownTime = 2.0f;
        public float durationTime = 0.8f;
        public float nextAttackTime = 0f;
        public float moveDistance = 0f;
        public float afterAttackDuration = 0f;
        public int damage = 0;
        public float rangeWidth = 0f;
        public float rangeHeight = 0f;
        public float damageDelay = 0f;
        public Vector2 rangeCenterOffset;
    }

    public AttackData slashAttack;
    public AttackData swingAttack;
    public AttackData dashAttack;
    public AttackData teleportAttack;
    private float startTime;
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

        Right_swing = 13,
        down_swing = 14,
        left_swing = 15,
        up_swing = 16,

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
        rangeRenderer = attackRangeIndicator.GetComponent<SpriteRenderer>();
        theAudio = FindObjectOfType<AudioManager>();
        HideAttackRange();
        bossTransform = this.transform;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slashAttack.range);
    }

    void Update()
    {
        if (isAttacking) return;

        Vector2 direction = playerTransform.position - transform.position;
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        float currentTime = Time.time;

        if (AllAttacksOnCooldown() || distance <= (slashAttack.range) && currentTime <= globalLastAttackTime + globalCooldown)
        {
            Idle(direction);
            return;
        }
        else
        {
            if (distance <= swingAttack.range && Time.time >= swingAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Swing(direction);
                currentAttack = swingAttack;
                swingAttack.nextAttackTime = Time.time + swingAttack.cooldownTime;
                globalLastAttackTime = currentTime + swingAttack.durationTime;
                ShowAttackRange(currentAttack.range, currentAttack, direction);
                StartCoroutine(HideAttackRangeAfterDelay(swingAttack.durationTime));
            }
            
            

            else if (distance >= dashAttack.minRange && distance <= dashAttack.range && Time.time >= dashAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Dash(direction);
                currentAttack = dashAttack;
                dashAttack.nextAttackTime = Time.time + dashAttack.cooldownTime;
                globalLastAttackTime = currentTime + dashAttack.durationTime;
                ShowAttackRange(currentAttack.range, currentAttack, direction);
                StartCoroutine(HideAttackRangeAfterDelay(dashAttack.durationTime));
            }

            else if (distance <= slashAttack.range && Time.time >= slashAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            {
                Slash(direction);
                currentAttack = slashAttack;
                slashAttack.nextAttackTime = Time.time + slashAttack.cooldownTime;
                globalLastAttackTime = currentTime + slashAttack.durationTime;
                ShowAttackRange(currentAttack.range, currentAttack, direction);
                StartCoroutine(HideAttackRangeAfterDelay(slashAttack.durationTime));
            }
            // else if (distance <= teleportAttack.range && Time.time >= teleportAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            // {
            //     Teleport(direction);
            //     currentAttack = teleportAttack;
            //     teleportAttack.nextAttackTime = Time.time + teleportAttack.cooldownTime + teleportAttack.afterAttackDuration + 5.0f;
            //     globalLastAttackTime = currentTime + teleportAttack.durationTime;
            //     StartCoroutine(TeleportMovement(direction));
            //     // StartCoroutine(HideAttackRangeAfterDelay(teleportAttack.durationTime));
            // }
            else
            {
                if (!isAttacking)
                {
                    FollowPlayer(direction);
                }
            }
        }
    }

    IEnumerator HideAttackRangeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideAttackRange();
    }

  void ShowAttackRange(float range, AttackData attackData, Vector2 direction)
    {
        Vector3 rangeOffset = attackData.rangeCenterOffset;

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && direction.y < 0)
        {
            float downwardAdjustment = 20f;
            rangeOffset.y -= downwardAdjustment;
        }

        if (attackData == swingAttack)
        {
            Vector3 newCenterPosition = bossTransform.position;
            rangeRenderer.sprite = rectangleRangeSprite;
            attackRangeIndicator.transform.position = newCenterPosition;

            Vector3 scale;
            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                scale = new Vector3(attackData.rangeHeight, attackData.rangeWidth, 1);
            }
            else
            {
                scale = new Vector3(attackData.rangeWidth, attackData.rangeHeight, 1);
            }

            attackRangeIndicator.transform.localScale = scale;
            rangeRenderer.enabled = true;

            StartCoroutine(DamageAfterDelay(attackData.damageDelay, attackData.damage, newCenterPosition, scale));
        }
        else
        {
            Vector3 newCenterPosition = bossTransform.position + rangeOffset;
            rangeRenderer.sprite = rectangleRangeSprite;
            attackRangeIndicator.transform.position = newCenterPosition;

            Vector3 scale;
            if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            {
                scale = new Vector3(attackData.rangeHeight, attackData.rangeWidth, 1);
            }
            else
            {
                scale = new Vector3(attackData.rangeWidth, attackData.rangeHeight, 1);
            }

            attackRangeIndicator.transform.localScale = scale;
            rangeRenderer.enabled = true;

            StartCoroutine(DamageAfterDelay(attackData.damageDelay, attackData.damage, newCenterPosition, scale));
        }
    }

    IEnumerator DamageAfterDelay(float delay, float damage, Vector3 position, Vector3 scale)
{
    yield return new WaitForSeconds(delay);

    // 데미지 판정을 수행하는 로직
    Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(position, scale, 0);
    foreach (Collider2D hit in hitPlayers)
    {
        if (hit.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 주는 코드
            PlayerStatus playerHealth = hit.GetComponent<PlayerStatus>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}

    void HideAttackRange()
    {
        rangeRenderer.enabled = false;
    }


    bool AllAttacksOnCooldown()
    {
        float currentTime = Time.time;
        return currentTime < slashAttack.nextAttackTime &&
               currentTime < swingAttack.nextAttackTime &&
               currentTime < dashAttack.nextAttackTime &&
               currentTime < teleportAttack.nextAttackTime;
    }

private IEnumerator DashMovement(Vector2 dashDistance)
{
    Vector2 dashStartPos = transform.position;
    Vector2 dashEndPos = dashStartPos + dashDistance;

    // 대쉬가 거의 끝날 때까지 기다립니다.
    yield return new WaitForSeconds(dashAttack.durationTime * 0.6f);

    // 대쉬의 마지막 부분에서 목표 위치로 빠르게 이동합니다.
    float dashEndTime = dashAttack.durationTime * 0.4f;
    float elapsedTime = 0;
    while (elapsedTime < dashEndTime)
    {
        transform.position = Vector2.Lerp(dashStartPos, dashEndPos, elapsedTime / dashEndTime);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    transform.position = dashEndPos;
    isAttacking = false;
}
    void SetAnimationState(Vector2 direction)
{
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
    {
        animator.SetInteger(animationsState, direction.x > 0 ? (int)States.dashRight : (int)States.dashLeft);
    }
    else
    {
        animator.SetInteger(animationsState, direction.y > 0 ? (int)States.dashBack : (int)States.dashFront);
    }
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
        ShowAttackRange(teleportAttack.range, teleportAttack, lookDirection);
        Vector2 calculatedOffset = CalculateCenterOffset(lookDirection, teleportAttack.rangeWidth, teleportAttack.rangeHeight);
        teleportAttack.rangeCenterOffset = calculatedOffset;
    }

    private IEnumerator TeleportMovement(Vector2 direction)
    {
        isAttacking = true;
        isTeleporting = true;

        yield return new WaitForSeconds(teleportAttack.durationTime);

        GetComponent<SpriteRenderer>().enabled = false;

        float waitTime = Random.Range(2f, 5f);
        yield return new WaitForSeconds(waitTime);

        Vector2 playerForward = playerTransform.TransformDirection(Vector2.up);
        Vector2 teleportPosition = (Vector2)playerTransform.position + (playerForward * teleportAttack.moveDistance); // 반대 방향으로 바꿉니다.

        transform.position = teleportPosition;

        GetComponent<SpriteRenderer>().enabled = true;

        Vector2 lookDirection = playerTransform.position - transform.position;
        LookAtPlayer(lookDirection);

        yield return new WaitForSeconds(teleportAttack.afterAttackDuration);

        isTeleporting = false;
    }

    void Idle(Vector2 direction)
    {
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
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
        }
        else
        {
            direction.x = 0;
        }

        direction.Normalize();

        rigidbody2D.velocity = direction * movementSpeed;
        
        UpdateState(direction);
    }

    Vector2 CalculateCenterOffset(Vector2 direction, float width, float height)
    {
        Vector2 normDirection = direction.normalized;

        float offsetX = normDirection.x * width * 0.5f;
        float offsetY = normDirection.y * height * 0.5f;

        return new Vector2(offsetX, offsetY);
    }

    void Slash(Vector2 direction)
    {
        isAttacking = true;

        rigidbody2D.velocity = Vector2.zero;
        theAudio.Play("Attack");
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

        Vector2 calculatedOffset = CalculateCenterOffset(direction, slashAttack.rangeWidth, slashAttack.rangeHeight);
        slashAttack.rangeCenterOffset = calculatedOffset;

        StartCoroutine(AttackCooldown(slashAttack.durationTime));

    }

    void Swing(Vector2 direction)
    {
        isAttacking = true;

        rigidbody2D.velocity = Vector2.zero;
        theAudio.Play("Swing");
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetInteger(animationsState, (int)States.Right_swing);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.left_swing);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetInteger(animationsState, (int)States.up_swing);
            }
            else
            {
                animator.SetInteger(animationsState, (int)States.down_swing);
            }
        }

        Vector2 calculatedOffset = CalculateCenterOffset(direction, swingAttack.rangeWidth, swingAttack.rangeHeight);
        swingAttack.rangeCenterOffset = calculatedOffset;

        StartCoroutine(AttackCooldown(swingAttack.durationTime));

    }

    void Dash(Vector2 direction)
{   
    
    isAttacking = true;

    rigidbody2D.velocity = Vector2.zero;
    theAudio.Play("Jump");
    // 방향에 따라 애니메이션 상태를 설정합니다.
     SetAnimationState(direction);

    Vector2 calculatedOffset = CalculateCenterOffset(direction, dashAttack.rangeWidth, dashAttack.rangeHeight);
    dashAttack.rangeCenterOffset = calculatedOffset;

    StartCoroutine(AttackCooldown(dashAttack.durationTime));
    StartCoroutine(DashMovement(direction.normalized * dashAttack.moveDistance));
}

    void Teleport(Vector2 direction)
    {
        isAttacking = true;

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

        StartCoroutine(AttackCooldown(teleportAttack.durationTime));

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
