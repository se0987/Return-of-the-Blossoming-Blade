using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill // 스킬 리스트로 추가 가능하도록
{
    public float Cooldown; //쿨타임
    public int Priority;  // 낮을수록 우선순위가 높음
    public float SkillAnimationTime; //애니메이션 실행 시간(스킬 발동 후 대기시간)
}

public class Boss3Controller : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float attackDistance;
    public int walkCount;
    public float LastUsedTime; // 마지막 스킬 사용 시간
    private float maxHP = 600f;
    public float currentHP;

    public Image bossHpBar;

    private Transform player;
    private Animator animator;

    private PlayerManager playerManager;
    public LayerMask layerMask;
    protected Vector2 direction;

    private int playTime = 0;

    public List<Skill> skillList;

    public Collider2D chunsalCollider;  // 천살 콜라이더 (피격)

    private bool isSkillActive = false; // 스킬이 활성화 중인지 여부
    private bool isAttack = false; // 공격 중인지 여부
    private bool isHit = false; // 피격 중인지 여부
    public bool boss3Moving = false; // 이동 여부
    private DCutScene4 dCutScene4;
    private bool searchPlayerMovingOne = false; //스크립트 진행 중에 멈추도록

    void Start()
    {
        dCutScene4 = FindObjectOfType<DCutScene4>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform; //플레이어 찾기
        playerManager = PlayerManager.instance; //플레이어 매니저의 인스턴스 가져오기

        chunsalCollider = GetComponent<Collider2D>(); // 천살 콜라이더 가져오기
        
        currentHP = maxHP; // 보스의 현재 피
        GameObject hpGaugeObject = GameObject.Find("Boss_HP_Gauge1");
        if (hpGaugeObject)
        {

            bossHpBar = hpGaugeObject.GetComponent<Image>();
        }

        PlayerPrefs.SetInt("havePosion", 100);
        LastUsedTime = -2f;
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
            dCutScene4.start = true;
            boss3Moving = true;
            searchPlayerMovingOne = false;
        }

        // 범위안에 있고, 공격중이 아닐 때 공격
        if (distanceToPlayer <= attackDistance && !isAttack)
        {
            PerformAttack();
        }

        // 플레이어 쫒기
        if (distanceToPlayer > stoppingDistance)
        {
            MoveTowardsPlayer(direction);
        }
        else
        {
            PerformAttack();
        }

        if (!boss3Moving)
        {
            animator.SetBool("IsMoving", false);
        }

        // 2페이즈
        if (currentHP <= 300)
        {
            dCutScene4.face2 = true;
        }

        if (bossHpBar != null && currentHP > 0)
        {
            float hpRatio = currentHP / maxHP;
            bossHpBar.fillAmount = hpRatio;
        }
    }

    // =========================================== (이동) ===========================================
    float GetDistanceToPlayer() // 플레이어와의 거리 계산
    {
        return Vector2.Distance(transform.position, player.position);
    }

    void UpdateAnimatorDirection(Vector2 direction) //보스 방향 설정
    {
        if (!isAttack)
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


    void MoveTowardsPlayer(Vector2 direction) //플레이어 쫒기
    {
        if (boss3Moving)
        {
            animator.SetBool("IsMoving", true);
            // 어느축으로 이동할지 설정
            bool moveOnXAxis = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) + 0.2f;

            // moveOnXAxis가 true인 경우 x 축으로만 이동하고 y 속도는 0으로 설정
            if (moveOnXAxis)
            {
                transform.Translate(new Vector2(direction.x * speed * Time.deltaTime, 0f));
            }
            // moveOnXAxis가 false인 경우 y 축으로만 이동하고 x 속도는 0으로 설정
            else
            {
                transform.Translate(new Vector2(0f, direction.y * speed * Time.deltaTime));
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }


    // =========================================== (공격) ===========================================
    void PerformAttack()//공격 실행
    {
        foreach (Skill skill in skillList)
        {
            if (Time.time - LastUsedTime >= 2f)
            {
                LastUsedTime = Time.time;
                StartCoroutine(ActivateAttackSkill(skill));
                break;
            }
        }
    }

    IEnumerator ActivateAttackSkill(Skill skill)// 스킬 스위칭 실행 ===================쿨타임 적용하기!!!
    {
        yield return StartCoroutine(PerformSkill(1, skill));

        //switch (skill.Priority)
        //{
        //    case 0:
        //        yield return StartCoroutine(PerformSkill(1, skill));
        //        break;
        //    case 1:
        //        yield return StartCoroutine(PerformSkill(2, skill));
        //        break;
        //    case 2:
        //        yield return StartCoroutine(PerformSkill(3, skill));
        //        break;
        //    default:
        //        break;
        //}
    }

    IEnumerator PerformSkill(int skillNumber, Skill skill) //스킬 실행
    {
        isAttack = true;
        boss3Moving = false;

        animator.SetTrigger("Attack" + skillNumber); // 스킬 애니메이션 활성화

        // 데미지는 콜라이더 스크립트 생성 후 직접 공격

        yield return new WaitForSeconds(skill.SkillAnimationTime);

        isAttack = false;
        boss3Moving = true;
    }

    private void OnTriggerEnter2D( Collider2D collision) // 피격
    {        
        if (collision.CompareTag("Weapon") && !isHit) // Weapon의 콜라이더에 부딪힌 경우
        {
            isHit = true; //중복 피격 막기
            WeaponDamage weapon = collision.GetComponent<WeaponDamage>();
            if (weapon)
            {
                currentHP -= weapon.damageAmount; // 무기의 데미지만큼 체력 깎기
            }
        
            if (currentHP <= 0) //현재 체력 0 이하면 죽기
            {
                Debug.Log("천살 전투 종료");
                boss3Moving = false;

                PlayerPrefs.SetFloat("ChunsalPlayTime", playTime);
                dCutScene4.end = true;
                dCutScene4.time = playTime;

                GetComponent<Boss3Controller>().enabled = false;
                gameObject.SetActive(false);

                return;
            }

            StartCoroutine(TakeDamageEffect()); //피격 후 경직/무적 설정
        }
    }

    IEnumerator TakeDamageEffect() // 경직 효과를 처리하는 Coroutine
    {
        boss3Moving = false;
        float waitTime = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            animator.SetTrigger("IsHit");
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }
        animator.SetTrigger("IsHit");

        isHit = false;
        boss3Moving = true;
    }




}