using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public float Cooldown;
    public float LastUsedTime;
    public int Priority;  // 낮을수록 우선순위가 높음
    public IEnumerator ActivationCoroutine;

    public float SkillRangeX;
    public float SkillRangeY;
    public float SkillDamage;
}

public class Boss3Controller : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float attackDistance;
    public int walkCount;

    private Transform player;
    private Animator animator;

    private PlayerManager playerManager;
    private Collider2D enemyCollider;
    private GameObject attackCollider;

    public List<Skill> skillList;

    private Coroutine currentAttackCoroutine;

    public float maxHP = 100f;
    private float currentHP;
    private bool isSkillActive = false; // 스킬이 활성화 중인지 여부

    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    protected Vector2 direction;
    public bool boss3Moving = false;
    private bool searchPlayerMovingOne = true;

    void Start()
    {
        // 초기화
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        attackCollider = transform.Find("Attack Collider").gameObject;
        attackCollider.SetActive(false);
        enemyCollider = GetComponent<Collider2D>();
        playerManager = PlayerManager.instance;
        currentHP = maxHP;
    }

    void Update()
    {
        // 플레이어와의 거리 계산
        float distanceToPlayer = GetDistanceToPlayer();

        // 플레이어 방향 계산
        direction = (player.position - transform.position).normalized;
        UpdateAnimatorDirection(direction);

        // 플레이어가 멈춘 상태이거나 
        if (!playerManager.notMove && searchPlayerMovingOne)
        {
            boss3Moving = true;
            searchPlayerMovingOne = false;
        }

        // 범위안에 공격
        if (distanceToPlayer <= attackDistance)
        {
            PerformAttack();
        }

        // 플레이어 쫒기
        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer(direction);

        }
    }

    void PerformAttack()
    {
        // 공격 가능 거리에 있으면 멈추고 공격
        //StartCoroutine(StopMoving());

        // 현재 공격 중이 아니라면 공격 실행
        foreach (Skill skill in skillList)
        {
            if (Time.time - skill.LastUsedTime >= skill.Cooldown && boss3Moving)
            {
                StartCoroutine(ActivateAttackSkill(skill));
                skill.LastUsedTime = Time.time;
                break;
            }
        }
    }

    void UpdateAnimatorDirection(Vector2 direction)
    {
        if (boss3Moving)
        {
            // 애니메이터의 방향 설정
            animator.SetFloat("DirX", direction.x);
            animator.SetFloat("DirY", direction.y);

            // 대각선 방향 조정
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                animator.SetFloat("DirY", 0f);
            }
            else
            {
                animator.SetFloat("DirX", 0f);
            }
        }
    }

    IEnumerator ActivateAttackSkill(Skill skill)
    {
        boss3Moving = false;
        animator.SetBool("IsRunning", false);
        switch (skill.Priority)
        {
            case 0:
                yield return StartCoroutine(PerformSkill(1, skill));
                break;
            case 1:
                yield return StartCoroutine(PerformSkill(2, skill));
                break;
            case 2:
                yield return StartCoroutine(PerformSkill(3, skill));
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1f);
        boss3Moving = true;
        // 스킬 사용이 끝난 후 다시 플레이어를 쫒을 수 있도록 이동 시작
        MoveTowardsPlayer((player.position - transform.position).normalized);
    }

    IEnumerator PerformSkill(int skillNumber, Skill skill)
    {
        animator.SetTrigger("Attack" + skillNumber);
        animator.SetTrigger("SkillLayer");

        // 스킬 동작 수행
        Debug.Log($"Using Skill {skillNumber} with RangeX: {skill.SkillRangeX}, RangeY: {skill.SkillRangeY}, Damage: {skill.SkillDamage}");

        // 스킬 범위 내의 대상에게 데미지 주기
        Vector2 skillStartPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skillStartPosition, skill.SkillDamage);

        //foreach (Collider2D collider in colliders)
        //{
        //    if (collider.CompareTag("Player"))
        //    {
        //        PlayerStatus PlayerStatus = collider.GetComponent<PlayerStatus>();
        //        if (PlayerStatus != null)
        //        {
        //            PlayerStatus.TakeDamage(skill.SkillDamage);
        //        }
        //    }
        //}

        // 스킬에 따라 다른 이펙트 생성
        switch (skillNumber)
        {
            case 1:
                // Instantiate(skillEffectPrefab1, skillStartPosition, Quaternion.identity);
                break;
            case 2:
                // Instantiate(skillEffectPrefab2, skillStartPosition, Quaternion.identity);
                break;
            case 3:
                // Instantiate(skillEffectPrefab3, skillStartPosition, Quaternion.identity);
                break;
            default:
                break;
        }


        yield return new WaitForSeconds(0.1f);
    }

    void MoveTowardsPlayer(Vector2 direction)
    {
        if (boss3Moving)
        {
            //StartCoroutine(Later());
            // playerManager.notMove가 false일 때만 이동
            // 직선 이동만 가능하도록 설정 -> 안됨! 수정!!!
            transform.Translate(direction * speed * Time.deltaTime);
            animator.SetBool("IsRunning", true);
        }
    }

    IEnumerator Later()
    {
        while (true)
        {
            // 막혔다면
            bool checkCollsionFlag = CheckCollsion();
            if (checkCollsionFlag)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
    }

    IEnumerator StopMoving()
    {
        // 이동을 멈추고 대기 애니메이션으로 전환
        //transform.position = this.transform.position; 
        boss3Moving = false;
        animator.SetBool("IsRunning", false);
        yield return new WaitForSeconds(30f);
        boss3Moving = true;
    }

    float GetDistanceToPlayer()
    {
        // 플레이어와의 거리 계산
        return Vector2.Distance(transform.position, player.position);
    }

    // 보스 패턴
    //void CheckPhaseTransition()
    //{
    //    float currentPercentage = (currentHP / maxHP) * 100f;

    //    //if (currentPercentage <= 40f && phase == 1)
    //    //{
    //    //    phase = 2;
    //    //    StartCoroutine(HealSkill());
    //    //}
    //}

    // 피격
    void TakeDamage(float damage)
    {
        // 데미지를 받았을 때 처리
        currentHP -= damage;

        // 여기에 데미지를 입었을 때의 처리를 추가할 수 있습니다.
        // 예를 들어, 피격 애니메이션 재생이나 경직 효과 등을 구현할 수 있습니다.

        // 경직 효과를 위해 Coroutine을 사용
        StartCoroutine(TakeDamageEffect());
    }

    // 경직 효과를 처리하는 Coroutine
    IEnumerator TakeDamageEffect()
    {
        // 이동 중인 애니메이션을 중지
        animator.SetBool("IsRunning", false);

        // 예시: 뒤로 살짝 밀리는 효과
        Vector2 knockbackDirection = (transform.position - player.position).normalized;

        // 예시: 뒤로 살짝 밀리는 효과를 직접 구현
        float knockbackSpeed = 5f;
        float knockbackDuration = 0.5f;
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            // 뒤로 밀리는 효과
            transform.Translate(knockbackDirection * knockbackSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // 경직 효과가 끝나면 Idle 애니메이션으로 전환
        animator.SetTrigger("Idle");

        // 경직 효과가 끝나면 이동을 다시 시작
        MoveTowardsPlayer((player.position - transform.position).normalized);
    }

    protected bool CheckCollsion()
    {
        RaycastHit2D hit;

        Vector2 start = this.transform.position;
        Vector2 end = start + new Vector2(direction.x * speed * walkCount, direction.y * speed * walkCount);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
        {
            return true;
        }
        else
        {
            Debug.Log("3");
            return false;
        }
    }
}
