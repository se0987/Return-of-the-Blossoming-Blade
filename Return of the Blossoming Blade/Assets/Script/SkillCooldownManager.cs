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

    public float QSkillDuration = 1.9f;
    public float WSkillDuration = 1.9f;
    public float ESkillDuration = 4.4f;
    public float SpaceDuration = 0.25f;

    public GameObject QSkillCol; // Q�� Skill Col ����

    public GameObject WSkillCol;
    public float WSkillDistance = 400f; // W ��ų ���󰡴� �Ÿ�
    private Vector2 lastDirection = Vector2.left;  // �⺻������ ������ �ٶ󺸰� ����
    public GameObject WSkillParticle;

    public GameObject ESkillCol;
    public GameObject ESkillCol2;

    public Animator playerAnimator; // �÷��̾� �ִϸ����� ����
    private bool isAttacking = false; // ���� ������ Ȯ���ϴ� ����

    public Collider2D playerCollider1; // �÷��̾� �ݶ��̴� ����
    public Collider2D playerCollider2; // �÷��̾� �ݶ��̴� ����

    public float dashDistance = 150f; // �뽬 �Ÿ�


    private void Update()
    {
        // ��ų�� Ȱ��ȭ ������ üũ
        bool anySkillActive = isQActive || isWActive || isEActive;

        if (Input.GetKeyDown(KeyCode.A) && !isAttacking && !anySkillActive)
        {
            StartCoroutine(Attack());
        }

        // �÷��̾��� ������ ������ �����մϴ�.
        if (Input.GetKeyDown(KeyCode.UpArrow)) lastDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) lastDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) lastDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) lastDirection = Vector2.right;

        // Q ��ų ���
        if (Input.GetKeyDown(KeyCode.Q) && !isQCooldown && !anySkillActive)
        {
            StartCoroutine(UseSkill(QCooldownTime, QSkillDuration, QBar, QSkillObject, () => isQActive = true, () => isQActive = false, () => isQCooldown = true, () => isQCooldown = false));
            StartCoroutine(QSkillEffect()); // QSkillEffect �ڷ�ƾ ȣ��

        }

        // W ��ų ���
        if (Input.GetKeyDown(KeyCode.W) && !isWCooldown && !anySkillActive)
        {
            StartCoroutine(UseSkill(WCooldownTime, WSkillDuration, WBar, WSkillObject, () => isWActive = true, () => isWActive = false, () => isWCooldown = true, () => isWCooldown = false));
            StartCoroutine(WSkillEffect());  // ��ƼŬ ȿ�� �ڷ�ƾ ȣ��
        }

        // E ��ų ���
        if (Input.GetKeyDown(KeyCode.E) && !isECooldown && !anySkillActive)
        {
            StartCoroutine(UseSkill(ECooldownTime, ESkillDuration, EBar, ESkillObject, () => isEActive = true, () => isEActive = false, () => isECooldown = true, () => isECooldown = false));
            StartCoroutine(ESkillEffect());
        }
        // �����̽��� ���
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking && !anySkillActive)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator UseSkill(float cooldownTime, float skillDuration, LoadingBarSegments bar, GameObject skillObject, Action onStart, Action onEnd, Action onCooldownStart, Action onCooldownEnd)
    {
        onStart();
        skillObject.SetActive(true);

        PlayerManager.instance.notMove = true; // ��ų�� Ȱ��ȭ�Ǹ� �̵��� �����ϴ�.

        // ��ų ���� �ð� ���� Ȱ��ȭ ���� ����
        yield return new WaitForSeconds(skillDuration);

        PlayerManager.instance.notMove = false; // ��ų ���� �ð��� ������ �̵��� ����մϴ�.

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
        for (int i = 0; i < 3; i++) // 3�� �ݺ�
        {
            QSkillCol.SetActive(true);  // Skill Col Ȱ��ȭ
            yield return new WaitForSeconds(0.3f); // 0.1�� ���. �� ���� �����Ͽ� ON/OFF ������ ������ �� �ֽ��ϴ�.
            QSkillCol.SetActive(false); // Skill Col ��Ȱ��ȭ
            yield return new WaitForSeconds(0.15f); // 0.1�� ���
        }
    }

    private IEnumerator WSkillEffect()
    {
        WSkillCol.SetActive(true);  // ��ų �ݶ��̴� Ȱ��ȭ

        Vector2 originalPos = WSkillCol.transform.position;
        Vector2 targetPos = originalPos + lastDirection * WSkillDistance;

        if (lastDirection == Vector2.left) // ������ �� z �� -90
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (lastDirection == Vector2.down) // �Ʒ����� �� z �� 0
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (lastDirection == Vector2.right) // �������� �� z �� 90
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (lastDirection == Vector2.up) // ������ �� z �� 180
        {
            WSkillParticle.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        float startTime = Time.time;
        float journeyLength = Vector2.Distance(originalPos, targetPos);

        float fractionOfJourney = 0f; // �ʱ� ������ 0���� ����

        while (fractionOfJourney < 1.0f) // 1�� ������ ������ �ݺ�
        {
            float timeSinceStarted = (Time.time - startTime) / WSkillDuration; // ���� �ð��� ��ų ���� �ð����� ������ ������ ���մϴ�.
            fractionOfJourney = 1 - Mathf.Pow(1 - timeSinceStarted, 3); // ������ ������, ���� �������� ȿ���� ����ϴ�.

            WSkillCol.transform.position = Vector2.Lerp(originalPos, targetPos, fractionOfJourney); // �ε巴�� ������ ��ġ�� �ݶ��̴� �̵�


            yield return null;
        }

        WSkillCol.transform.position = targetPos;  // ���� �ð��� ������ ��ǥ �������� �ݶ��̴� ��ġ ����

        WSkillCol.SetActive(false);  // ��ų ȿ�� ���� �� �ݶ��̴� ��Ȱ��ȭ

        WSkillCol.transform.position = originalPos;  // �ݶ��̴��� ������ ��ġ�� �缳��
    }
    private IEnumerator ESkillEffect()
    {
        ESkillCol.SetActive(true); // ù ��° �ݶ��̴� Ȱ��ȭ
        ESkillCol2.SetActive(true); // �� ��° �ݶ��̴� Ȱ��ȭ

        yield return new WaitForSeconds(ESkillDuration); // 4.4�� ���� ���

        ESkillCol.SetActive(false); // ù ��° �ݶ��̴� ��Ȱ��ȭ
        ESkillCol2.SetActive(false); // �� ��° �ݶ��̴� ��Ȱ��ȭ
    }

    // �⺻ ���� �ڷ�ƾ
    private IEnumerator Attack()
    {
        isAttacking = true; // ���� ���·� ����
        PlayerManager.instance.notMove = true; // �÷��̾��� �̵� ���

        // �������� AttackH �Ǵ� AttackV Ʈ���� ����
        if (UnityEngine.Random.value < 0.5f)
        {
            playerAnimator.SetTrigger("AttackH");
        }
        else
        {
            playerAnimator.SetTrigger("AttackV");
        }

        // ���� �ִϸ��̼� �ð���ŭ ���
        yield return new WaitForSeconds(1.04f);

        // �ִϸ��̼� �Ҹ� ���� �ʱ�ȭ
        playerAnimator.SetBool("AttackH", false);
        playerAnimator.SetBool("AttackV", false);

        PlayerManager.instance.notMove = false; // �÷��̾��� �̵� ��� ����
        isAttacking = false; // ���� ���� ����
    }

    private IEnumerator Dash()
    {

        PlayerManager.instance.notMove = true; // �̵� ���
        playerCollider1.enabled = false;
        playerCollider2.enabled = false;// �ݶ��̴� ��Ȱ��ȭ

        playerAnimator.SetBool("Dash", true); // Dash �ִϸ��̼� Ȱ��ȭ

        Vector2 originalPosition = PlayerManager.instance.transform.position;
        Vector2 targetPosition = originalPosition + lastDirection * dashDistance; // dashDistance�� �̵��� �Ÿ��� �����ؾ� ��

        float startTime = Time.time;
        while (Time.time - startTime < SpaceDuration)
        {
            float fractionOfJourney = (Time.time - startTime) / SpaceDuration;
            // PlayerManager�� MovePlayer �Լ��� ����� �÷��̾� ��ġ ������Ʈ
            PlayerManager.instance.MovePlayer(Vector2.Lerp(originalPosition, targetPosition, fractionOfJourney));
            yield return null;
        }

        // ���� �������� �������� �� �÷��̾� ��ġ ����
        PlayerManager.instance.MovePlayer(targetPosition);

        playerCollider1.enabled = true;
        playerCollider2.enabled = true; // �ݶ��̴� Ȱ��ȭ
        PlayerManager.instance.notMove = false; // �̵� ��� ����
        playerAnimator.SetBool("Dash", false); // Dash �ִϸ��̼� ��Ȱ��ȭ

    }
}