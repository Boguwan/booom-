using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public float moveSpeed = 3f;          // Boss�ƶ��ٶ�
    public float wanderRadius = 5f;        // Χ�����ǵ����ΰ뾶
    public Transform playerTransform;      // �������ǵ�Transform

    private Vector2 targetPosition;        // ��ǰĿ��λ��

    void Start()
    {
        
        
        SetNewDestination();
    }

    void Update()
    {
        if (playerTransform == null) return; // ���ǲ�����ʱֹͣ�߼�

        // ��Ŀ��λ���ƶ�
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        // ����Ŀ���������λ��
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        // ������Ϊ�����������Ŀ���
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        targetPosition = (Vector2)playerTransform.position + randomOffset;
    }
}
