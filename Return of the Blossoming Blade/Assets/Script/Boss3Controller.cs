using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss3Controller : MonoBehaviour
{
    public float speed;
    public float stoppingDistance; // 유지거리
    public float attackDistance; // 공격 가능 거리

    private Transform player;
    private Animator animator;

    private PlayerManager playerManager; // 플레이어 매니저 참조

    public Collider2D enemyCollider;  // 적의 콜라이더를 참조할 변수
    private GameObject attackCollider;  // 공격 콜라이더

    // 쿨다운 시간 설정
    public float AttackSkill1Cooldown = 8f;
    public float AttackSkill2Cooldown = 13f;
    public float AttackSkill3Cooldown = 30f;

    // 각 스킬의 마지막 사용 시간 기록
    private float lastAttackSkill1Time = 0f;
    private float lastAttackSkill2Time = 0f;
    private float lastAttackSkill3Time = 0f;

    public float damageToPlayer = 20f; // 데미지
    private float knockbackDuration = 0.2f; // 넉백


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 태그 찾기
        animator = GetComponent<Animator>();
        attackCollider = transform.Find("Attack Collider").gameObject;
        attackCollider.SetActive(false);
        enemyCollider = GetComponent<Collider2D>();  // 적의 콜라이더를 가져옵니다.
        playerManager = PlayerManager.instance; // PlayerManager의 인스턴스를 가져옵니다.

    }

    void Update()
    {
        // PlayerManager의 notMove 변수가 true이면 아래 로직을 실행하지 않습니다.
        if (playerManager.notMove)
            return;

        float distanceToPlayer = GetDistanceToPlayer(); // 플레이어와의 거리

        Vector2 direction = (player.position - transform.position).normalized;
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);

        if (distanceToPlayer > stoppingDistance) // 유지 거리보다 멀리 있다면
        {
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= attackDistance) // 공격 거리 안이라면
        {
            StopMoving();

            StartCoroutine(PerformRandomAttackSkill());
        }
    }


    IEnumerator PerformRandomAttackSkill()
    {
        int randomAttack = Random.Range(0, 3);
        float cooldown = 0f;
        float lastSkillTime = 0f;

        switch (randomAttack)
        {
            case 0:
                cooldown = AttackSkill1Cooldown;
                lastSkillTime = lastAttackSkill1Time;
                break;
            case 1:
                cooldown = AttackSkill2Cooldown;
                lastSkillTime = lastAttackSkill2Time;
                break;
            case 2:
                cooldown = AttackSkill3Cooldown;
                lastSkillTime = lastAttackSkill3Time;
                break;
            default:
                break;
        }

        if (Time.time - lastSkillTime >= cooldown)
        {
            yield return StartCoroutine(ActivateAttackSkill(randomAttack));

            // Update the last used time based on the chosen skill
            switch (randomAttack)
            {
                case 0:
                    lastAttackSkill1Time = Time.time;
                    break;
                case 1:
                    lastAttackSkill2Time = Time.time;
                    break;
                case 2:
                    lastAttackSkill3Time = Time.time;
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator ActivateAttackSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                yield return StartCoroutine(AttackSkill1());
                break;
            case 1:
                yield return StartCoroutine(AttackSkill2());
                break;
            case 2:
                yield return StartCoroutine(AttackSkill3());
                break;
            default:
                break;
        }
    }

    IEnumerator AttackSkill1()
    {
        StopMoving();
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.4f); // 애니메이션 실행 후 0.4초 대기

        //Debug.Log("1");


        // 콜라이더 활성화
        attackCollider.SetActive(true);

        yield return new WaitForSeconds(0.3f); // 0.3초 동안 유지

        // 콜라이더 비활성화
        attackCollider.SetActive(false);

    }

    IEnumerator AttackSkill2()
    {
        StopMoving();
        animator.SetTrigger("Attack2");
        yield return new WaitForSeconds(0.4f); // 애니메이션 실행 후 0.4초 대기

        //Debug.Log("2");


        // 콜라이더 활성화
        attackCollider.SetActive(true);

        yield return new WaitForSeconds(0.3f); // 0.3초 동안 유지

        // 콜라이더 비활성화
        attackCollider.SetActive(false);

    }

    IEnumerator AttackSkill3()
    {
        StopMoving();
        animator.SetTrigger("Attack3");
        yield return new WaitForSeconds(0.4f); // 애니메이션 실행 후 0.4초 대기

        //Debug.Log("3");


        // 콜라이더 활성화
        attackCollider.SetActive(true);

        yield return new WaitForSeconds(0.3f); // 0.3초 동안 유지

        // 콜라이더 비활성화
        attackCollider.SetActive(false);

    }


    float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        animator.SetBool("IsRunning", true);
    }

    void StopMoving()
    {
        transform.position = this.transform.position;
        animator.SetBool("IsRunning", false);
    }
}