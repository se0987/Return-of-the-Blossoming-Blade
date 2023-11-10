using System;
using System.Collections;
using UnityEngine;
using Modularify.LoadingBars3D;

public class SkillCooldownManager : MonoBehaviour
{
    public LoadingBarSegments QBar;
    public LoadingBarSegments WBar;
    public LoadingBarSegments EBar;
    public LoadingBarSegments SpaceBar;


    public GameObject QSkillObject;
    public GameObject WSkillObject;
    public GameObject ESkillObject;

    public float QCooldownTime = 5f;
    public float WCooldownTime = 10f;
    public float ECooldownTime = 30f;

    private bool isQCooldown = false;
    private bool isWCooldown = false;
    private bool isECooldown = false;

    private bool isQActive = false;
    private bool isWActive = false;
    private bool isEActive = false;

    public float QSkillDuration = 1.6f;
    public float WSkillDuration = 1.9f;
    public float ESkillDuration = 4.4f;
    public float SpaceDuration = 0.25f;

    public GameObject QSkillCol; // Q의 Skill Col 참조

    public GameObject WSkillCol;
    public float WSkillDistance = 400f; // W 스킬 날라가는 거리
    private Vector2 lastDirection = Vector2.left;  // 기본적으로 왼쪽을 바라보게 설정
    public GameObject WSkillParticle;

    public GameObject ESkillCol;
    public GameObject ESkillCol2;

    public Animator playerAnimator; // 플레이어 애니메이터 참조
    private bool isAttacking = false; // 공격 중인지 확인하는 변수

    public Collider2D playerCollider1; // 플레이어 콜라이더 참조
    public Collider2D playerCollider2; // 플레이어 콜라이더 참조

    public float dashDistance = 185f; // 대쉬 거리

    public PlayerStatus playerStatus; // PlayerStatus 참조

    public float QSkillMP = 5f;
    public float WSkillMP = 5f;
    public float ESkillMP = 15f;

    public float SpaceCooldownTime = 3f; // 스페이스바 스킬의 쿨다운 시간

    private bool isSpaceCooldown = false; // 스페이스바 스킬 쿨다운 상태

    private void Update()
    {
        // 스킬이 활성화 중인지 체크
        bool anySkillActive = isQActive || isWActive || isEActive;

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking && !anySkillActive)
        {
            StartCoroutine(Attack());
        }

        // 플레이어의 마지막 방향을 추적합니다.
        if (Input.GetKeyDown(KeyCode.UpArrow)) lastDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) lastDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) lastDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) lastDirection = Vector2.right;

        // Q 스킬 사용
        if (Input.GetKeyDown(KeyCode.Q) && !isQCooldown && !anySkillActive)
        {
           if (playerStatus.currentMP >= QSkillMP) // 충분한 MP가 있는지 확인
            {
                playerStatus.UseMP(QSkillMP); // MP 소모
                StartCoroutine(UseSkill(QCooldownTime, QSkillDuration, QBar, QSkillObject, () => isQActive = true, () => isQActive = false, () => isQCooldown = true, () => isQCooldown = false));
                StartCoroutine(QSkillEffect());
            }

        }

        // W 스킬 사용
        if (Input.GetKeyDown(KeyCode.W) && !isWCooldown && !anySkillActive)
        {
            if (playerStatus.currentMP >= WSkillMP)
            {
                playerStatus.UseMP(WSkillMP);
                StartCoroutine(UseSkill(WCooldownTime, WSkillDuration, WBar, WSkillObject, () => isWActive = true, () => isWActive = false, () => isWCooldown = true, () => isWCooldown = false));
                StartCoroutine(WSkillEffect());
            }
        }

        // E 스킬 사용
        if (Input.GetKeyDown(KeyCode.E) && !isECooldown && !anySkillActive)
        {
            if (playerStatus.currentMP >= ESkillMP)
            {
                playerStatus.UseMP(ESkillMP);
                StartCoroutine(UseSkill(ECooldownTime, ESkillDuration, EBar, ESkillObject, () => isEActive = true, () => isEActive = false, () => isECooldown = true, () => isECooldown = false));
                StartCoroutine(ESkillEffect());
            }
        }
        // 스페이스바 사용
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && !anySkillActive && !PlayerManager.instance.skillNotMove && !PlayerManager.instance.notMove && !isSpaceCooldown)
        {
            StartCoroutine(Dash());
            StartCoroutine(SpaceCooldown(SpaceCooldownTime, SpaceBar, () => isSpaceCooldown = true, () => isSpaceCooldown = false));
        }
    }

    private IEnumerator UseSkill(float cooldownTime, float skillDuration, LoadingBarSegments bar, GameObject skillObject, Action onStart, Action onEnd, Action onCooldownStart, Action onCooldownEnd)
    {
        onStart();
        skillObject.SetActive(true);

        PlayerManager.instance.skillNotMove = true; // 스킬이 활성화되면 이동을 막습니다.

        // 스킬 지속 시간 동안 활성화 상태 유지
        yield return new WaitForSeconds(skillDuration);

        PlayerManager.instance.skillNotMove = false; // 스킬 지속 시간이 끝나면 이동을 허용합니다.

        skillObject.SetActive(false);
        onEnd();

        onCooldownStart();
        float timeElapsed = 0;
        while (timeElapsed < cooldownTime)
        {
            timeElapsed += Time.deltaTime;
            float percentage = timeElapsed / cooldownTime;
            bar.SetPercentage(percentage);
            yield return null;
        }
        bar.SetPercentage(0);
        onCooldownEnd();
    }

    private IEnumerator QSkillEffect()
    {
        playerAnimator.SetBool("SkillQ", true);

        for (int i = 0; i < 3; i++) // 3번 반복
        {
            QSkillCol.SetActive(true);  // Skill Col 활성화
            yield return new WaitForSeconds(0.3f); // 0.1초 대기. 이 값을 조절하여 ON/OFF 간격을 변경할 수 있습니다.
            QSkillCol.SetActive(false); // Skill Col 비활성화
            yield return new WaitForSeconds(0.15f); // 0.1초 대기
        }
        playerAnimator.SetBool("SkillQ", false);
    }

    private IEnumerator WSkillEffect()
    {
        WSkillCol.SetActive(true);  // 스킬 콜라이더 활성화

        Vector2 originalPos = WSkillCol.transform.position;
        Vector2 targetPos = originalPos + lastDirection * WSkillDistance;

        if (lastDirection == Vector2.left) // 왼쪽일 때 z 값 -90
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (lastDirection == Vector2.down) // 아래쪽일 때 z 값 0
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (lastDirection == Vector2.right) // 오른쪽일 때 z 값 90
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (lastDirection == Vector2.up) // 위쪽일 때 z 값 180
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        playerAnimator.SetBool("SkillW", true);
        float startTime = Time.time;
        float journeyLength = Vector2.Distance(originalPos, targetPos);

        float fractionOfJourney = 0f; // 초기 비율을 0으로 설정

        while (fractionOfJourney < 1.0f) // 1에 도달할 때까지 반복
        {
            float timeSinceStarted = (Time.time - startTime) / WSkillDuration; // 지난 시간을 스킬 지속 시간으로 나누어 비율을 구합니다.
            fractionOfJourney = 1 - Mathf.Pow(1 - timeSinceStarted, 3); // 시작이 빠르게, 끝에 느려지는 효과를 만듭니다.

            WSkillCol.transform.position = Vector2.Lerp(originalPos, targetPos, fractionOfJourney); // 부드럽게 보간된 위치로 콜라이더 이동


            yield return null;
        }

        WSkillCol.transform.position = targetPos;  // 지속 시간이 끝나면 목표 지점으로 콜라이더 위치 설정

        WSkillCol.SetActive(false);  // 스킬 효과 종료 후 콜라이더 비활성화
        playerAnimator.SetBool("SkillW", false);


        WSkillCol.transform.position = originalPos;  // 콜라이더를 원래의 위치로 재설정
    }
    private IEnumerator ESkillEffect()
    {
        playerAnimator.SetBool("SkillE", true);

        ESkillCol.SetActive(true); // 첫 번째 콜라이더 활성화
        ESkillCol2.SetActive(true); // 두 번째 콜라이더 활성화

        yield return new WaitForSeconds(ESkillDuration); // 4.4초 동안 대기

        ESkillCol.SetActive(false); // 첫 번째 콜라이더 비활성화
        ESkillCol2.SetActive(false); // 두 번째 콜라이더 비활성화

        playerAnimator.SetBool("SkillE", false);

    }

    // 기본 공격 코루틴
    private IEnumerator Attack()
    {
        isAttacking = true; // 공격 상태로 설정
        PlayerManager.instance.skillNotMove = true; // 플레이어의 이동 잠금

        // 랜덤으로 AttackH 또는 AttackV 트리거 설정
        if (UnityEngine.Random.value < 0.5f)
        {
            playerAnimator.SetBool("AttackH", true);
        }
        else
        {
            playerAnimator.SetBool("AttackV", true);
        }

        // 공격 애니메이션 시간만큼 대기
        yield return new WaitForSeconds(1.04f);

        // 애니메이션 불린 값을 초기화
        playerAnimator.SetBool("AttackH", false);
        playerAnimator.SetBool("AttackV", false);

        PlayerManager.instance.skillNotMove = false; // 플레이어의 이동 잠금 해제
        isAttacking = false; // 공격 상태 해제
    }

    private IEnumerator Dash()
    {

        PlayerManager.instance.skillNotMove = true; // 이동 잠금
        playerCollider1.enabled = false;
        playerCollider2.enabled = false;// 콜라이더 비활성화

        playerAnimator.SetBool("Dash", true); // Dash 애니메이션 활성화

        Vector2 originalPosition = PlayerManager.instance.transform.position;
        Vector2 targetPosition = originalPosition + lastDirection * dashDistance; // dashDistance는 이동할 거리를 정의해야 함

        float startTime = Time.time;
        while (Time.time - startTime < SpaceDuration)
        {
            float fractionOfJourney = (Time.time - startTime) / SpaceDuration;
            // PlayerManager의 MovePlayer 함수를 사용해 플레이어 위치 업데이트
            PlayerManager.instance.MovePlayer(Vector2.Lerp(originalPosition, targetPosition, fractionOfJourney));
            yield return null;
        }

        // 최종 목적지에 도달했을 때 플레이어 위치 고정
        PlayerManager.instance.MovePlayer(targetPosition);

        playerCollider1.enabled = true;
        playerCollider2.enabled = true; // 콜라이더 활성화
        PlayerManager.instance.skillNotMove = false; // 이동 잠금 해제
        playerAnimator.SetBool("Dash", false); // Dash 애니메이션 비활성화

    }
    private IEnumerator SpaceCooldown(float cooldownTime, LoadingBarSegments bar, Action onStartCooldown, Action onEndCooldown)
    {
        onStartCooldown?.Invoke();
        float timeElapsed = 0;
        while (timeElapsed < cooldownTime)
        {
            timeElapsed += Time.deltaTime;
            float percentage = timeElapsed / cooldownTime;
            bar.SetPercentage(percentage);
            yield return null;
        }
        bar.SetPercentage(0);
        onEndCooldown?.Invoke();
    }
}