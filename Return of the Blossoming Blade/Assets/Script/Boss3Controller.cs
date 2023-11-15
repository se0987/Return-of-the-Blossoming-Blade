using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill // 스킬 리스트로 추가 가능하도록
{
    public float Cooldown; //쿨타임
    public int Priority;  // 낮을수록 우선순위가 높음 + 스킬 번호
    public float SkillAnimationTime; //애니메이션 실행 시간(스킬 발동 후 대기시간)
    public float SkillDamage;

    public Collider2D SkillCollider; // 스킬 콜라이더
    //public IEnumerator ActivationCoroutine;
}

public class Boss3Controller : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float attackDistance;
    public int walkCount;
    public float LastUsedTime; // 마지막 스킬 사용 시간
    private float maxHP = 1000f;
    public float currentHP;

    private Transform player;
    private Animator animator;

    private PlayerManager playerManager;
    public LayerMask layerMask;
    protected Vector2 direction;

    public List<Skill> skillList;

    //private GameObject attackCollider; // 공격 콜라이더 (공격)
    public Collider2D chunsalCollider;  // 천살 콜라이더 (피격)
    public GameObject attackRangeIndicator; // 콜라이더 범위 오브젝트


    private bool isSkillActive = false; // 스킬이 활성화 중인지 여부
    private bool isHit = false; // 피격 중인지 여부
    public bool boss3Moving = false; // 이동 여부
    private DCutScene4 dCutScene4;
    private bool searchPlayerMovingOne = true; //스크립트 진행 중에 멈추도록

    void Start()
    {
        dCutScene4 = FindObjectOfType<DCutScene4>();
        // 초기화
        player = GameObject.FindGameObjectWithTag("Player").transform; //플레이어 찾기
        animator = GetComponent<Animator>();

        //attackCollider = transform.Find("Attack Collider").gameObject; //
        //attackCollider.SetActive(false); // 공격 콜라이더 비활성화
        chunsalCollider = GetComponent<Collider2D>(); // 천살 콜라이더 가져오기
        
        playerManager = PlayerManager.instance; //플레이어 매니저의 인스턴스 가져오기
        currentHP = maxHP; // 보스의 현재 피
                           //rangeRenderer = attackRangeIndicator.GetComponent<SpriteRenderer>(); //공격범위

        LastUsedTime = -2f;
        ///////////////////////////////수정필요////////////////////합칠때풀기
        //dCutScene4.Start = true;
        ///////////////////////////////수정필요////////////////////합칠때 지우기
        PlayerPrefs.SetInt("havePosion", 100);
    }

    void Update()
    {
        // 플레이어와의 거리 계산
        float distanceToPlayer = GetDistanceToPlayer();

        // 플레이어 방향 계산
        direction = (player.position - transform.position).normalized;
        UpdateAnimatorDirection(direction);

        // 스크립트 진행 중에 멈추도록
        if (!playerManager.notMove && searchPlayerMovingOne)
        {
            boss3Moving = true;
            searchPlayerMovingOne = false;
        }

        //CheckCollsion(chunsalCollider);

        // 범위안에 있다면 공격
        if (distanceToPlayer <= attackDistance)
        {
            boss3Moving = false;
            PerformAttack();
            boss3Moving = true;
        }

        // 플레이어 쫒기
        if (distanceToPlayer > stoppingDistance)
        {
            // 범위안에 있다면 공격
            if (distanceToPlayer <= attackDistance)
            {
                boss3Moving = false;
                PerformAttack();
                boss3Moving = true;
            }
            boss3Moving = true;
            MoveTowardsPlayer(direction);
        }
        else
        {
            boss3Moving = false;
            animator.SetBool("IsRunning", false);
            PerformAttack();
        }

    }

    // =========================================== (이동) ===========================================
    float GetDistanceToPlayer() // 플레이어와의 거리 계산
    {
        return Vector2.Distance(transform.position, player.position);
    }
    
    //IEnumerator StopMoving() // 이동 멈추기
    //{
    //    // 이동을 멈추고 대기 애니메이션으로 전환
    //    //transform.position = this.transform.position; 
    //    boss3Moving = false;
    //    animator.SetBool("IsRunning", false);
    //    yield return new WaitForSeconds(5f);
    //    boss3Moving = true;
    //}

    void UpdateAnimatorDirection(Vector2 direction) //보스 방향 설정
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

    //IEnumerator Later() // 보스 콜라이드 막기
    //{
    //    while (true)
    //    {
    //        // 막혔다면
    //        bool checkCollsionFlag = CheckCollsion();
    //        if (checkCollsionFlag)
    //        {
    //            yield return new WaitForSeconds(1f);
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //}

    void MoveTowardsPlayer(Vector2 direction) //플레이어 쫒기
    {
        if (boss3Moving)
        {
            // 어느축으로 이동할지 설정
            bool moveOnXAxis = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) + 0.2f;

            // moveOnXAxis가 true인 경우 x 축으로만 이동하고 y 속도는 0으로 설정
            if (moveOnXAxis)
            {
                transform.Translate(new Vector2(direction.x * speed * Time.deltaTime, 0f));
                animator.SetBool("IsRunning", true);
            }
            // moveOnXAxis가 false인 경우 y 축으로만 이동하고 x 속도는 0으로 설정
            else
            {
                transform.Translate(new Vector2(0f, direction.y * speed * Time.deltaTime));
                animator.SetBool("IsRunning", true);
            }
        }
    }


    // =========================================== (공격) ===========================================
    void PerformAttack()//공격 실행
    {
        foreach (Skill skill in skillList)
        {
            if (Time.time - LastUsedTime -2f >= skill.Cooldown)
            {
                StartCoroutine(ActivateAttackSkill(skill));
                LastUsedTime = Time.time;
                break;
            }
        }
    }

    IEnumerator ActivateAttackSkill(Skill skill)// 스킬 스위칭 실행
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

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++수정필요+++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        yield return new WaitForSeconds(0.5f); // 스킬 끝난 후 잠깐 기다리기

        // 스킬 사용이 끝난 후 다시 플레이어를 쫒을 수 있도록 이동 시작
        boss3Moving = true;
        MoveTowardsPlayer((player.position - transform.position).normalized);
    }

    IEnumerator PerformSkill(int skillNumber, Skill skill) // 실제 스킬 실행
    {
        // 애니메이션 트리거
        animator.SetTrigger("Attack" + skillNumber);

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++수정필요+++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //// 스킬 범위 내의 모든 콜라이더 찾기
        //Collider2D[] colliders = Physics2D.OverlapAreaAll(

        //);

        //// 찾은 콜라이더들에 대해 반복
        //foreach (Collider2D collider in colliders)
        //{
        //    // "playercombatcol" 태그를 가진 콜라이더
        //    if (collider.CompareTag("PlayerCombatCol"))
        //    {
        //        PlayerStatus playerStatus = collider.transform.parent.GetComponent<PlayerStatus>();
        //        if (playerStatus != null)
        //        {
        //            playerStatus.TakeDamage(skill.SkillDamage);
        //        }
        //    }
        //}
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++수정필요+++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        yield return new WaitForSeconds(0.1f);
    }

    private void OnTriggerEnter2D( Collider2D collision) // 피격
    {
        if (currentHP <= 0) //현재 체력 0 이하면 죽기
        {
            Debug.Log("천살 전투 종료");
            boss3Moving = false;

            ///////////////////////////////수정필요////////////////////합칠때풀기
            //PlayerPrefs.SetFloat("ChunsalPlayTime", playTime);
            //dCutScene4.End = true;
            //dCutScene4.Time = playTime;

            GetComponent<Boss3Controller>().enabled = false;
            gameObject.SetActive(false);

            return;
        }

        if (collision.CompareTag("Weapon") && !isHit) // Weapon의 콜라이더에 부딪힌 경우
        {
            isHit = true; //중복 피격 막기
            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                currentHP -= weapon.damageAmount; // 무기의 데미지만큼 체력 깎기
            }
            StartCoroutine(TakeDamageEffect()); //피격 후 경직/무적 설정
        }
    }

    IEnumerator TakeDamageEffect() // 경직 효과를 처리하는 Coroutine
    {
        float waitTime = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            animator.SetTrigger("IsHit");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
        animator.SetTrigger("IsHit");


        isHit = false;
    }
}