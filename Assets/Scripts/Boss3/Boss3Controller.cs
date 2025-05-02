using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : MonoBehaviour
{
    // 剑刃预制体，用于发射剑刃攻击玩家
    public GameObject swordPrefab;
    // 剑刃发射点的 Transform 组件，决定剑刃生成的位置和方向
    public Transform firePoint;
    // 剑刃发射的时间间隔，单位为秒
    public float fireRate = 3f;
    // Boss 的最大生命值
    public float maxHealth = 100f;
    // Boss 当前的生命值，私有变量，外部不能直接访问
    private float currentHealth;
    // Boss 预制体，用于在 Boss 分裂时创建新的 Boss 分身
    public GameObject bossPrefab;
    // 玩家的 Transform 组件，用于确定分裂的圆心位置
    public Transform playerTransform;
    // 分裂时分身所在圆的半径
    public float splitRadius = 5f;
    private bool isSplit=false;

    public float moveSpeed = 3f;
    public float wanderRadius = 2f;
    private Vector2 targetPosition;

    // 在脚本实例被启用时调用，初始化 Boss 的当前生命值并开始发射剑刃的协程
    void Start()
    {
        SetNewDestination();
        // 将当前生命值初始化为最大生命值
        currentHealth = maxHealth;
        // 启动发射剑刃的协程
        StartCoroutine(FireSwords());
    }

    // 每帧调用一次，检查 Boss 的生命值是否降到一半以下，如果是则触发分裂
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
        // 检查当前生命值是否小于等于最大生命值的一半且大于 0
        if (currentHealth <= maxHealth / 2 && currentHealth > 0&&!isSplit)
        {
            // 调用分裂方法
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
    /// 协程方法，用于每隔一定时间发射剑刃
    /// </summary>
    /// <returns>协程迭代器</returns>
    IEnumerator FireSwords()
    {
        // 无限循环，持续发射剑刃
        while (true)
        {
            // 等待指定的时间间隔
            yield return new WaitForSeconds(fireRate);
            // 计算从发射点到玩家的方向
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            // 计算旋转角度
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            // 在发射点的位置和旋转角度实例化剑刃预制体
            GameObject sword = Instantiate(swordPrefab, firePoint.position, rotation);
            // 获取剑刃的刚体组件并设置其速度
            Rigidbody2D swordRb = sword.GetComponent<Rigidbody2D>();
            if (swordRb != null)
            {
                swordRb.velocity = direction * 10f;
            }
            Destroy(sword,5f);
        }
    }

    /// <summary>
    /// 公共方法，用于处理 Boss 受到伤害的逻辑
    /// </summary>
    /// <param name="damage">受到的伤害值</param>
    public void TakeDamage(float damage)
    {
        // 从当前生命值中减去受到的伤害
        currentHealth -= damage;
        // 检查当前生命值是否小于等于 0
        if (currentHealth <= 0)
        {
            // 调用死亡方法
            StartCoroutine(Die());
        }
    }


    /// <summary>
    /// 分裂方法，当 Boss 的生命值降到一半以下时，将 Boss 分裂成五个分身，以玩家为圆心的圆上生成
    /// </summary>
    void Split()
    {
        // 计算每个分身之间的角度间隔
        float angleStep = 360f / 5;
        for (int i = 0; i < 5; i++)
        {
            // 计算当前分身的角度
            float angle = i * angleStep;
            // 将角度转换为弧度
            float radian = angle * Mathf.Deg2Rad;
            // 计算分身的位置
            Vector2 positionOffset = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * splitRadius;
            Vector3 spawnPosition = playerTransform.position + (Vector3)positionOffset;

            // 在计算出的位置实例化一个新的 Boss 分身
            GameObject newBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            // 获取新 Boss 分身的 BossController 组件
            Boss3Controller newBossController = newBoss.GetComponent<Boss3Controller>();
            // 将新 Boss 分身的最大生命值设置为当前 Boss 剩余生命值的五分之一
            newBossController.maxHealth = currentHealth / 5;
            // 将新 Boss 分身的当前生命值设置为当前 Boss 剩余生命值的五分之一
            newBossController.currentHealth = currentHealth / 5;
            // 为新 Boss 分身设置玩家 Transform 组件
            newBossController.playerTransform = playerTransform;
        }
        // 启动协程等待子弹发射完毕后销毁当前 Boss 对象
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
    /// 死亡方法，当 Boss 的生命值降为 0 时，等待子弹发射完毕后销毁 Boss 对象
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
