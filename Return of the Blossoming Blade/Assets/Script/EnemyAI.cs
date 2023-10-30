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
    Dead
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




    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        animator = GetComponent<Animator>();

        attackCollider = transform.Find("Attack Collider").gameObject;

        attackCollider.SetActive(false);  // 처음엔 비활성화
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
                break;
            case EnemyState.Attacking:
                // 플레이어를 공격하는 로직
                StartCoroutine(AttackSequence());
                break;
            case EnemyState.Hit:
                // 피해를 받는 애니메이션 및 로직
                break;
            case EnemyState.Dead:
                // 죽음 애니메이션 및 로직
                break;
        }
    }

    IEnumerator MoveAfterDelay()
    {
        yield return new WaitForSeconds(aggroTimer);
        if (currentState == EnemyState.Aggro) // Aggro 상태 확인
        {
            ChangeState(EnemyState.Moving);
        }
    }

    IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(0.4f);  // 애니메이션 실행 후 0.4초 대기
        attackCollider.SetActive(true);  // 콜라이더 활성화
        yield return new WaitForSeconds(0.3f);  // 0.3초 동안 유지
        attackCollider.SetActive(false);  // 다시 비활성화
        if (distanceToPlayer <= attackRange)  // 공격 범위 내에 있을 때
        {
            ChangeState(EnemyState.Aggro);  // 다시 Aggro 상태로
        }
        else
        {
            ChangeState(EnemyState.Moving);  // 그렇지 않으면 이동 상태로
        }
    }

    private Vector2 GetDirectionToPlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

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
        if (hit.collider != null && hit.collider.tag != "Player")
        {
            return true;
        }
        return false;
    }

    private void MoveInDirection(Vector2 direction)
    {
        transform.Translate(direction * speed * Time.deltaTime);
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
                break;
            case EnemyState.Moving:
                animator.SetInteger("State", 2);
                break;
            case EnemyState.Attacking:
                animator.SetInteger("State", 3);
                break;
            case EnemyState.Hit:
                animator.SetInteger("State", 4);
                break;
            case EnemyState.Dead:
                animator.SetInteger("State", 5);
                break;
        }
    }
}