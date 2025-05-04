using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = 3f;          // Boss移动速度
    public float wanderRadius = 5f;        // 围绕主角的漫游半径
    public Transform playerTransform;      // 引用主角的Transform

    private Vector2 targetPosition;        // 当前目标位置

    void Start()
    {
        
        
        SetNewDestination();
    }

    void Update()
    {
        if (playerTransform == null) return; // 主角不存在时停止逻辑

        // 向目标位置移动
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // 到达目标后生成新位置
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        // 以主角为中心生成随机目标点
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        targetPosition = (Vector2)playerTransform.position + randomOffset;
    }
}
