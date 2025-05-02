using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    // ����Ԥ���壬���ڷ��佣�й������
    public GameObject swordPrefab;
    // ���з����� Transform ����������������ɵ�λ�úͷ���
    public Transform firePoint;
    // ���з����ʱ��������λΪ��
    public float fireRate = 3f;
    // Boss ���������ֵ
    public float maxHealth = 100f;
    // Boss ��ǰ������ֵ��˽�б������ⲿ����ֱ�ӷ���
    private float currentHealth;
    // Boss Ԥ���壬������ Boss ����ʱ�����µ� Boss ����
    public GameObject bossPrefab;
    // ��ҵ� Transform ���������ȷ�����ѵ�Բ��λ��
    public Transform playerTransform;
    // ����ʱ��������Բ�İ뾶
    public float splitRadius = 5f;
    private bool isSplit=false;

    public float moveSpeed = 3f;
    public float wanderRadius = 2f;
    private Vector2 targetPosition;

    // �ڽű�ʵ��������ʱ���ã���ʼ�� Boss �ĵ�ǰ����ֵ����ʼ���佣�е�Э��
    void Start()
    {
        SetNewDestination();
        // ����ǰ����ֵ��ʼ��Ϊ�������ֵ
        currentHealth = maxHealth;
        // �������佣�е�Э��
        StartCoroutine(FireSwords());
    }

    // ÿ֡����һ�Σ���� Boss ������ֵ�Ƿ񽵵�һ�����£�������򴥷�����
    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewDestination();
        }
        // ��鵱ǰ����ֵ�Ƿ�С�ڵ����������ֵ��һ���Ҵ��� 0
        if (currentHealth <= maxHealth / 2 && currentHealth > 0&&!isSplit)
        {
            // ���÷��ѷ���
            Split();
            isSplit=true;
        }
    }

    void SetNewDestination()
    {
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        targetPosition = (Vector2)transform.position + randomDirection;
    }

    /// <summary>
    /// Э�̷���������ÿ��һ��ʱ�䷢�佣��
    /// </summary>
    /// <returns>Э�̵�����</returns>
    IEnumerator FireSwords()
    {
        // ����ѭ�����������佣��
        while (true)
        {
            // �ȴ�ָ����ʱ����
            yield return new WaitForSeconds(fireRate);
            // ����ӷ���㵽��ҵķ���
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            // ������ת�Ƕ�
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // �ڷ�����λ�ú���ת�Ƕ�ʵ��������Ԥ����
            GameObject sword = Instantiate(swordPrefab, firePoint.position, rotation);
            // ��ȡ���еĸ���������������ٶ�
            Rigidbody2D swordRb = sword.GetComponent<Rigidbody2D>();
            if (swordRb != null)
            {
                swordRb.velocity = direction * 10f;
            }
            Destroy(sword,5f);
        }
    }

    /// <summary>
    /// �������������ڴ��� Boss �ܵ��˺����߼�
    /// </summary>
    /// <param name="damage">�ܵ����˺�ֵ</param>
    public void TakeDamage(float damage)
    {
        // �ӵ�ǰ����ֵ�м�ȥ�ܵ����˺�
        currentHealth -= damage;
        // ��鵱ǰ����ֵ�Ƿ�С�ڵ��� 0
        if (currentHealth <= 0)
        {
            // ������������
            StartCoroutine(Die());
        }
    }


    /// <summary>
    /// ���ѷ������� Boss ������ֵ����һ������ʱ���� Boss ���ѳ�������������ΪԲ�ĵ�Բ������
    /// </summary>
    void Split()
    {
        // ����ÿ������֮��ĽǶȼ��
        float angleStep = 360f / 5;
        for (int i = 0; i < 5; i++)
        {
            // ���㵱ǰ����ĽǶ�
            float angle = i * angleStep;
            // ���Ƕ�ת��Ϊ����
            float radian = angle * Mathf.Deg2Rad;
            // ��������λ��
            Vector2 positionOffset = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * splitRadius;
            Vector3 spawnPosition = playerTransform.position + (Vector3)positionOffset;

            // �ڼ������λ��ʵ����һ���µ� Boss ����
            GameObject newBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            // ��ȡ�� Boss ����� BossController ���
            Boss3Controller newBossController = newBoss.GetComponent<Boss3Controller>();
            // ���� Boss ������������ֵ����Ϊ��ǰ Boss ʣ������ֵ�����֮һ
            newBossController.maxHealth = currentHealth / 5;
            // ���� Boss ����ĵ�ǰ����ֵ����Ϊ��ǰ Boss ʣ������ֵ�����֮һ
            newBossController.currentHealth = currentHealth / 5;
            // Ϊ�� Boss ����������� Transform ���
            newBossController.playerTransform = playerTransform;
        }
        // ����Э�̵ȴ��ӵ�������Ϻ����ٵ�ǰ Boss ����
        StartCoroutine(WaitForBulletsAndDestroy());
    }

    IEnumerator WaitForBulletsAndDestroy()
    {
        Boss3Minion minionController = GetComponent<Boss3Minion>();
        if (minionController != null)
        {
            yield return StartCoroutine(minionController.FireBullets());
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// ������������ Boss ������ֵ��Ϊ 0 ʱ���ȴ��ӵ�������Ϻ����� Boss ����
    /// </summary>
    IEnumerator Die()
    {
        Boss3Minion minionController = GetComponent<Boss3Minion>();
        if (minionController != null)
        {
            yield return StartCoroutine(minionController.FireBullets());
        }
        Destroy(gameObject);
    }
}
