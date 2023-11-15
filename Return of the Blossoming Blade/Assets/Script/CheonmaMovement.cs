using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheonmaMovement : MonoBehaviour
{
    public Transform playerTransform;

    private bool isAttacking = false;

    private float globalLastAttackTime = 0f;
    private float globalCooldown = 1f;

    public GameObject attackRangeIndicator;
    private SpriteRenderer rangeRenderer;
    public Sprite rectangleRangeSprite;

    public AttackData currentAttack = null;


    [System.Serializable]
    public class AttackData
    {
        public float range = 50.0f;
        public float cooldownTime = 2.0f;
        public float durationTime = 0.8f;
        public float nextAttackTime = 0f;
        public float afterAttackDuration = 0f;
        public int damage = 0;
        public float rangeWidth = 0f;
        public float rangeHeight = 0f;
        public Vector2 rangeCenterOffset;
        public GameObject effectPrefab;
        public GameObject rangeIndicatorPrefab;
    }

    public AttackData leftHandAttack;
    public AttackData rightHandAttack;
    public AttackData bothHandAttack;
    public AttackData glowingEyeAttack;
    public AttackData redAttack;
    public AttackData yellowAttack;
    public AttackData greenAttack;
    public AttackData rainingAttack;

    Vector2 movement = new Vector2();
    Rigidbody2D rigidbody2D;

    Animator animator;
    string animationsState = "PatternState";

    enum States
    {
        idle = 1,
        leftHand = 2,
        rightHand = 3,
        bothHand = 4,
        redEnergy = 5,
        yellowEnergy = 6,
        greenEnergy = 7,
        glowEye = 8,
        raining = 9
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        rangeRenderer = attackRangeIndicator.GetComponent<SpriteRenderer>();
        HideAttackRange();
    }

    void Update()
    {
        if (isAttacking) return;

        float currentTime = Time.time;

        if (AllAttacksOnCooldown() || currentTime <= globalLastAttackTime + globalCooldown)
        {
            Idle();
            return;
        }

        List<Action> availableAttacks = new List<Action>();

        if (Time.time >= leftHandAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(leftHandAttack));
        if (Time.time >= rightHandAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(rightHandAttack));
        if (Time.time >= bothHandAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(bothHandAttack));
        if (Time.time >= redAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(redAttack));
        if (Time.time >= yellowAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(yellowAttack));
        if (Time.time >= greenAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(greenAttack));
        if (Time.time >= glowingEyeAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(glowingEyeAttack));
        if (Time.time >= rainingAttack.nextAttackTime && currentTime >= globalLastAttackTime + globalCooldown)
            availableAttacks.Add(() => PerformAttack(rainingAttack));

        if (availableAttacks.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableAttacks.Count);
            availableAttacks[randomIndex]();
        }
        else
        {
            if (!isAttacking)
            {
                Idle();
            }
        }
    }

    void PerformAttack(AttackData attackData)
    {
        currentAttack = attackData;
        attackData.nextAttackTime = Time.time + attackData.cooldownTime;
        globalLastAttackTime = Time.time + attackData.durationTime;

        ShowAttackRange(attackData.range, attackData);
        StartCoroutine(HideAttackRangeAfterDelay(attackData.durationTime));

        switch (attackData)
        {
            case var _ when attackData == leftHandAttack:
                LeftHand();
                break;
            case var _ when attackData == rightHandAttack:
                RightHand();
                break;
            case var _ when attackData == bothHandAttack:
                BothHand();
                break;
            case var _ when attackData == redAttack:
                Red();
                break;
            case var _ when attackData == yellowAttack:
                Yellow();
                break;
            case var _ when attackData == greenAttack:
                Green();
                break;
            case var _ when attackData == glowingEyeAttack:
                GlowingEye();
                break;
            case var _ when attackData == rainingAttack:
                Raining();
                break;
        }
    }

    void Idle()
    {
        animator.SetInteger(animationsState, (int)States.idle);
    }

    void LeftHand()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.leftHand);

        Vector2 calculatedOffset = CalculateCenterOffset(leftHandAttack.rangeWidth, leftHandAttack.rangeHeight);
        leftHandAttack.rangeCenterOffset = calculatedOffset;

        Vector3 attackPosition = playerTransform.position;

        GameObject rangeIndicatorInstance = Instantiate(leftHandAttack.rangeIndicatorPrefab, attackPosition, Quaternion.identity);
        rangeIndicatorInstance.transform.localScale = new Vector3(leftHandAttack.rangeWidth, leftHandAttack.rangeHeight, 1);

        StartCoroutine(ExecuteAfterAnimation(leftHandAttack.durationTime, () => {
            GameObject effectInstance = Instantiate(leftHandAttack.effectPrefab, attackPosition, Quaternion.identity);
            effectInstance.GetComponent<Collider2D>().isTrigger = true;
            effectInstance.tag = "BossAttack";

            rangeIndicatorInstance.transform.SetParent(effectInstance.transform);

            StartCoroutine(DisableEffect(effectInstance, leftHandAttack.durationTime));
        }));

        StartCoroutine(AttackCooldown(leftHandAttack.durationTime));
    }

    void RightHand()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.rightHand);

        Vector2 calculatedOffset = CalculateCenterOffset(rightHandAttack.rangeWidth, rightHandAttack.rangeHeight);
        rightHandAttack.rangeCenterOffset = calculatedOffset;

        Vector3 attackPosition = playerTransform.position;

        GameObject rangeIndicatorInstance = Instantiate(rightHandAttack.rangeIndicatorPrefab, attackPosition, Quaternion.identity);
        rangeIndicatorInstance.transform.localScale = new Vector3(rightHandAttack.rangeWidth, rightHandAttack.rangeHeight, 1);

        StartCoroutine(ExecuteAfterAnimation(rightHandAttack.durationTime, () => {
            GameObject effectInstance = Instantiate(rightHandAttack.effectPrefab, attackPosition, Quaternion.identity);
            effectInstance.GetComponent<Collider2D>().isTrigger = true;
            effectInstance.tag = "BossAttack";

            rangeIndicatorInstance.transform.SetParent(effectInstance.transform);

            StartCoroutine(DisableEffect(effectInstance, rightHandAttack.durationTime));
        }));

        StartCoroutine(AttackCooldown(rightHandAttack.durationTime));
    }

    void BothHand()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.bothHand);

        Vector2 calculatedOffset = CalculateCenterOffset(bothHandAttack.rangeWidth, bothHandAttack.rangeHeight);
        bothHandAttack.rangeCenterOffset = calculatedOffset;

        Vector3 attackPosition = playerTransform.position;

        GameObject rangeIndicatorInstance = Instantiate(bothHandAttack.rangeIndicatorPrefab, attackPosition, Quaternion.identity);
        rangeIndicatorInstance.transform.localScale = new Vector3(bothHandAttack.rangeWidth, bothHandAttack.rangeHeight, 1);

        StartCoroutine(ExecuteAfterAnimation(bothHandAttack.durationTime, () => {
            GameObject effectInstance = Instantiate(bothHandAttack.effectPrefab, attackPosition, Quaternion.identity);
            effectInstance.GetComponent<Collider2D>().isTrigger = true;
            effectInstance.tag = "BossAttack";

            rangeIndicatorInstance.transform.SetParent(effectInstance.transform);

            StartCoroutine(DisableEffect(effectInstance, bothHandAttack.durationTime));
        }));

        StartCoroutine(AttackCooldown(bothHandAttack.durationTime));
    }

    void Red()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.redEnergy);

        Vector2 calculatedOffset = CalculateCenterOffset(redAttack.rangeWidth, redAttack.rangeHeight);
        redAttack.rangeCenterOffset = calculatedOffset;

        Vector3 attackPosition = playerTransform.position;

        GameObject rangeIndicatorInstance = Instantiate(redAttack.rangeIndicatorPrefab, attackPosition, Quaternion.identity);
        rangeIndicatorInstance.transform.localScale = new Vector3(redAttack.rangeWidth, redAttack.rangeHeight, 1);

        StartCoroutine(ExecuteAfterAnimation(redAttack.durationTime, () => {
            GameObject effectInstance = Instantiate(redAttack.effectPrefab, attackPosition, Quaternion.identity);
            effectInstance.GetComponent<Collider2D>().isTrigger = true;
            effectInstance.tag = "BossAttack";

            rangeIndicatorInstance.transform.SetParent(effectInstance.transform);

            StartCoroutine(DisableEffect(effectInstance, redAttack.afterAttackDuration));
        }));

        StartCoroutine(AttackCooldown(redAttack.durationTime));
    }

    void Yellow()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.yellowEnergy);

        Vector2 calculatedOffset = CalculateCenterOffset(yellowAttack.rangeWidth, yellowAttack.rangeHeight);
        yellowAttack.rangeCenterOffset = calculatedOffset;

        Vector3 attackPosition = playerTransform.position;

        GameObject rangeIndicatorInstance = Instantiate(yellowAttack.rangeIndicatorPrefab, attackPosition, Quaternion.identity);
        rangeIndicatorInstance.transform.localScale = new Vector3(yellowAttack.rangeWidth, yellowAttack.rangeHeight, 1);

        StartCoroutine(ExecuteAfterAnimation(yellowAttack.durationTime, () => {
            GameObject effectInstance = Instantiate(yellowAttack.effectPrefab, attackPosition, Quaternion.identity);
            effectInstance.GetComponent<Collider2D>().isTrigger = true;
            effectInstance.tag = "BossAttack";

            rangeIndicatorInstance.transform.SetParent(effectInstance.transform);

            StartCoroutine(DisableEffect(effectInstance, yellowAttack.durationTime));
        }));

        StartCoroutine(AttackCooldown(yellowAttack.durationTime));
    }

    void Green()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.greenEnergy);

        Vector2 calculatedOffset = CalculateCenterOffset(greenAttack.rangeWidth, greenAttack.rangeHeight);
        greenAttack.rangeCenterOffset = calculatedOffset;

        Vector3 attackPosition = playerTransform.position;

        GameObject rangeIndicatorInstance = Instantiate(greenAttack.rangeIndicatorPrefab, attackPosition, Quaternion.identity);
        rangeIndicatorInstance.transform.localScale = new Vector3(greenAttack.rangeWidth, greenAttack.rangeHeight, 1);

        StartCoroutine(ExecuteAfterAnimation(greenAttack.durationTime, () => {
            GameObject effectInstance = Instantiate(greenAttack.effectPrefab, attackPosition, Quaternion.identity);
            effectInstance.GetComponent<Collider2D>().isTrigger = true;
            effectInstance.tag = "BossAttack";

            rangeIndicatorInstance.transform.SetParent(effectInstance.transform);

            StartCoroutine(DisableEffect(effectInstance, greenAttack.durationTime));
        }));

        StartCoroutine(AttackCooldown(greenAttack.durationTime));
    }

    void GlowingEye()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.glowEye);

        Vector2 calculatedOffset = CalculateCenterOffset(glowingEyeAttack.rangeWidth, glowingEyeAttack.rangeHeight);
        glowingEyeAttack.rangeCenterOffset = calculatedOffset;

        if (playerTransform != null)
        {
            Rigidbody2D playerRigidbody = playerTransform.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                StartCoroutine(TemporarilyIncreaseGravity(playerRigidbody, 10.0f, 3.0f));
            }
        }

        StartCoroutine(AttackCooldown(glowingEyeAttack.durationTime));
    }

    IEnumerator TemporarilyIncreaseGravity(Rigidbody2D playerRigidbody, float increasedGravity, float duration)
    {
        float originalGravity = playerRigidbody.gravityScale;
        playerRigidbody.gravityScale = increasedGravity;

        // 색상 변경을 위한 SpriteRenderer 참조
        SpriteRenderer spriteRenderer = playerTransform.GetComponent<SpriteRenderer>();
        Color originalColor = Color.white;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // 원래 색상 저장
            spriteRenderer.color = Color.magenta; // 보라색으로 변경
        }

        yield return new WaitForSeconds(duration);

        playerRigidbody.gravityScale = 0;
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);

        // 원래 색상으로 복원
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    void Raining()
    {
        isAttacking = true;

        animator.SetInteger(animationsState, (int)States.raining);

        int numberOfAttacks = 50; // 동시에 발생할 공격의 수
        Vector2 mapSize = new Vector2(1000, 700); // 맵의 크기 (가로, 세로)

        for (int i = 0; i < numberOfAttacks; i++)
        {
            StartCoroutine(ExecuteAfterAnimation(rainingAttack.durationTime - 2f, () => {
                Vector2 randomPosition = new Vector2(
                    UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2),
                    UnityEngine.Random.Range(-mapSize.y / 2, mapSize.y / 2)
                );

                // 범위 표시 생성
                GameObject rangeIndicatorInstance = Instantiate(rainingAttack.rangeIndicatorPrefab, randomPosition, Quaternion.identity);
                rangeIndicatorInstance.transform.localScale = new Vector3(rainingAttack.rangeWidth, rainingAttack.rangeHeight, 1);

                // 1초 후에 공격 이펙트 생성
                StartCoroutine(ExecuteAfterDelay(1f, () => {
                    GameObject effectInstance = Instantiate(rainingAttack.effectPrefab, randomPosition, Quaternion.identity);
                    effectInstance.GetComponent<Collider2D>().isTrigger = true;
                    effectInstance.tag = "BossAttack";

                    rangeIndicatorInstance.transform.SetParent(effectInstance.transform);
                    StartCoroutine(DisableEffect(effectInstance, 1f));
                }));
            }));
        }

        StartCoroutine(AttackCooldown(rainingAttack.durationTime));
    }

    IEnumerator ExecuteAfterDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    IEnumerator DisableEffect(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(effect);
    }

    IEnumerator ExecuteAfterAnimation(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    void ShowAttackRange(float range, AttackData attackData)
    {
        Vector3 rangeOffset = attackData.rangeCenterOffset;

        Vector3 newCenterPosition = playerTransform.position + rangeOffset;
        rangeRenderer.sprite = rectangleRangeSprite;
        attackRangeIndicator.transform.position = newCenterPosition;

        rangeRenderer.enabled = true;
    }

    IEnumerator HideAttackRangeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideAttackRange();
    }

    void HideAttackRange()
    {
        rangeRenderer.enabled = false;
    }

    bool AllAttacksOnCooldown()
    {
        float currentTime = Time.time;
        return currentTime < leftHandAttack.nextAttackTime &&
               currentTime < rightHandAttack.nextAttackTime &&
               currentTime < bothHandAttack.nextAttackTime &&
               currentTime < redAttack.nextAttackTime &&
               currentTime < yellowAttack.nextAttackTime &&
               currentTime < greenAttack.nextAttackTime &&
               currentTime < glowingEyeAttack.nextAttackTime &&
               currentTime < rainingAttack.nextAttackTime;
    }

    Vector2 CalculateCenterOffset(float width, float height)
    {

        float offsetX = width * 0.5f;
        float offsetY = height * 0.5f;

        return new Vector2(offsetX, offsetY);
    }

    private IEnumerator AttackCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }
}
