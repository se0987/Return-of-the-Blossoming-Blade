using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Aggro,
    Moving,
    Attacking,
    Hit,
    Dead,
    Dashing
}


public class EnemyAI : MonoBehaviour
{
    public float speed = 140f;
    public float detectionDistance = 55f;  // Raycasting 거리
    private Transform playerTransform;
    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Animator animator;

    public float aggroRange = 650f;  // 플레이어를 감지하는 거리

    public float distanceToPlayer; // 플레이어와의 거리를 저장할 public 변수

    public EnemyState currentState = EnemyState.Idle; //적 상태 변수

    private float aggroTimer = 0.5f;

    public float attackRange = 45f;  // 플레이어를 공격할 수 있는 거리

    private GameObject attackCollider;  // 공격 콜라이더

    private bool isAttacking = false;

    public float enemyHP = 100f; // 적의 HP

    public float damageToPlayer = 20f; // 플레이어에게 줄 데미지

    private float knockbackDuration = 0.2f; // 밀려나는 시간

    public bool canDash = false;  // 적이 도약(돌진) 할 수 있는지 여부를 결정하는 변수

    public float dashProbability = 0.3f;  // 도약(돌진)할 확률

    public float dashSpeed = 500f;  // 도약(돌진) 속도

    public float dashHeight = 250f;  // 도약(돌진) 중 Y축으로 얼마나 높이 점프할 것인지

    public float dashDuration = 0.8f;  // 도약(돌진)의 지속시간 (초)

    private bool hasDashed = false; // 적이 이미 도약(돌진)를 했는지 확인하는 변수

    private float dashTime = 0f;  // 도약(돌진)하는 동안 경과한 시간

    public Collider2D enemyCollider;  // 적의 콜라이더를 참조할 변수

    public float knockbackForce; // 원하는 힘의 크기를 설정합니다. (값은 조절 가능)


    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        attackCollider = transform.Find("Attack Collider").gameObject;

        attackCollider.SetActive(false);  // 처음엔 비활성화..

        enemyCollider = GetComponent<Collider2D>();  // 적의 콜라이더를 가져옵니다.

    }

    private void Update()
    {

        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position); // 플레이어와의 거리 계산

        switch (currentState)
        {
            case EnemyState.Idle:
                // Idle 상태의 로직
                if (distanceToPlayer <= aggroRange)
                {
                    ChangeState(EnemyState.Aggro);
                }

                break;
            case EnemyState.Aggro:
                // AggroedIdle 상태의 로직
                if (distanceToPlayer > aggroRange)
                {
                    ChangeState(EnemyState.Idle);
                }
                else
                {
                    StartCoroutine(MoveAfterDelay());
                }
                break;
            case EnemyState.Moving:
                // 플레이어를 향해 이동하는 로직
                Vector2 currentDirection = GetDirectionToPlayer();
                MoveInDirection(currentDirection);
                if (distanceToPlayer > aggroRange)
                {
                    ChangeState(EnemyState.Idle);
                }
                if (distanceToPlayer <= attackRange)  // 공격 범위 내에 있을 때
                {
                    ChangeState(EnemyState.Attacking);
                }
                // 도약(돌진) 로직 추가:
                if (canDash && !hasDashed && Random.value < dashProbability)
                {
                    hasDashed = true;
                    dashTime = 0f;  // 도약(돌진) 시간 초기화
                    ChangeState(EnemyState.Dashing);
                }
                break;
            case EnemyState.Attacking:
                // 플레이어를 공격하는 로직
                if (!isAttacking) // 공격 중이 아니라면 코루틴 호출
                {
                    StartCoroutine(AttackSequence());
                }
                break;
            case EnemyState.Hit:
                // 피해를 받는 애니메이션 및 로직
                StartCoroutine(HitSequence());
                break;
            case EnemyState.Dead:
                // 죽음 애니메이션 및 로직
                break;
            case EnemyState.Dashing:
                // 플레이어를 향해 도약(돌진)하는 로직
                DashTowardsPlayer();

                if (dashTime > dashDuration)
                {
                    ChangeState(EnemyState.Moving); // 도약(돌진) 지속시간이 끝나면 Moving 상태로 변경
                }

                break;

        }
    }

    IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(aggroTimer);
        if (currentState == EnemyState.Aggro) // Aggro 상태 확인
        {
            if (distanceToPlayer <= attackRange)  // 공격 범위 내에 있을 때
            {
                ChangeState(EnemyState.Attacking);
            }
            else
            {
                ChangeState(EnemyState.Moving);
            }
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.4f);  // 애니메이션 실행 후 0.4초 대기
        attackCollider.SetActive(true);  // 콜라이더 활성화
        yield return new WaitForSeconds(0.3f);  // 0.3초 동안 유지
        attackCollider.SetActive(false);  // 다시 비활성화

        isAttacking = false; // 코루틴이 끝났으므로 공격 중 상태를 해제

        if (distanceToPlayer > attackRange)
        {
            ChangeState(EnemyState.Aggro); // 공격이 끝나면 Aggro 상태로 전환
        }
        else
        {
            ChangeState(EnemyState.Idle); // 아니라면 일시적으로 Idle 상태로 전환하여 애니메이션을 재실행하게 합니다.
        }
    }

    private Vector2 GetDirectionToPlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // 대쉬 중인 경우 바로 플레이어를 향한 방향을 반환
        if (currentState == EnemyState.Dashing)
        {
            return directionToPlayer;
        }

        // 우선적으로 플레이어를 향한 방향으로 움직일 것인지 확인합니다.
        if (!IsObstacleInDirection(directionToPlayer))
        {
            return directionToPlayer;
        }

        // 플레이어를 향한 방향에 장애물이 있다면, 다른 가능한 방향으로 움직입니다.
        foreach (Vector2 dir in directions)
        {
            if (!IsObstacleInDirection(dir))
            {
                return dir;
            }
        }

        return Vector2.zero;  // 만약 모든 방향에 장애물이 있다면 움직이지 않습니다.
    }

    private bool IsObstacleInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance);
        if (hit.collider != null
            && hit.collider.tag != "Player"
            && hit.collider.tag != "Weapon"
            && hit.collider.tag != "Enemy")  // Enemy 태그를 무시하는 조건을 추가합니다.
        {
            return true;
        }
        return false;
    }

    private void MoveInDirection(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    IEnumerator HitSequence()
    {
        // 맞았을 때의 애니메이션 및 로직
        animator.SetInteger("State", 4); // Hit 애니메이션 실행
        attackCollider.SetActive(false);
        yield return new WaitForSeconds(0.2f); // 0.2초 지속
        OnHit(); // 밀리는 로직 호출
        if (enemyHP <= 0)
        {
            ChangeState(EnemyState.Dead); // 만약 적의 HP가 0 이하라면 죽음 상태로 전환
        }
        else
        {
            ChangeState(EnemyState.Idle); // 아니라면 Idle 상태로 전환
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && currentState != EnemyState.Hit) // Weapon의 콜라이더에 부딪힌 경우
        {
            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                enemyHP -= weapon.damageAmount; // 무기의 데미지만큼 체력 깎기
                knockbackForce = 5 + Mathf.Sqrt(weapon.damageAmount);
                ChangeState(EnemyState.Hit); // 체력이 아직 남아있다면, 피격 상태로 변경
            }
        }
    }

    private void OnHit()
    {
        Vector2 knockbackDirection = (transform.position - playerTransform.position).normalized; // 플레이어와의 반대 방향 계산

        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(StopKnockback()); // 밀려나는 효과 멈추기
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 속도 0으로 설정
    }

    private void DashTowardsPlayer()
    {
        Vector2 dashDirection = GetDirectionToPlayer();

        // Y축 "점프" 움직임 계산
        float yOffset = Mathf.Sin(dashTime * 2 * Mathf.PI) * dashHeight;

        if (dashDirection.x > 0)
        {   
            transform.Translate(new Vector3(dashSpeed * Time.deltaTime, yOffset * Time.deltaTime, 0f));
        }
        else if (dashDirection.x < 0)
        {
            transform.Translate(new Vector3(-dashSpeed * Time.deltaTime, yOffset * Time.deltaTime, 0f));
        }

        dashTime += Time.deltaTime;
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetInteger("State", 0);
                break;
            case EnemyState.Aggro:
                animator.SetInteger("State", 1);

                // 플레이어가 적보다 오른쪽에 있는지 체크
                if (playerTransform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);  // 오른쪽을 보도록
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);  // 왼쪽을 보도록
                }
                break;
            case EnemyState.Moving:
                animator.SetInteger("State", 2);
                enemyCollider.isTrigger = false;  // 돌진이 끝나면 콜라이더를 원래대로 설정
                // 플레이어가 적보다 오른쪽에 있는지 체크
                if (playerTransform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);  // 오른쪽을 보도록
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);  // 왼쪽을 보도록
                }
                break;
            case EnemyState.Attacking:
                animator.SetInteger("State", 3);
                break;
            case EnemyState.Hit:
                animator.SetInteger("State", 4);
                break;
            case EnemyState.Dead:
                animator.SetInteger("State", 5);
                GetComponent<BoxCollider2D>().enabled = false; // Box Collider 2D 비활성화
                break;
            case EnemyState.Dashing:
                animator.SetInteger("State", 6);
                enemyCollider.isTrigger = true;  // 돌진 중에는 콜라이더를 통과하도록 설정
                break;
        }
    }
}