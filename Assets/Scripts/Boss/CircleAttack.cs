using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    public GameObject redCirclePrefab;
    public int circleCount = 5;
    public float circleRadius = 1f;

    public void StartCircleAttack()
    {
        StartCoroutine(CircleAttackRoutine());
    }

    IEnumerator CircleAttackRoutine()
    {
        // ���ɺ�Ȧ
        List<GameObject> circles = new List<GameObject>();
        for (int i = 0; i < circleCount; i++)
        {
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * 5f;
            GameObject circle = Instantiate(redCirclePrefab, randomPos, Quaternion.identity);
            circles.Add(circle);
        }

        // �ȴ�1�����
        yield return new WaitForSeconds(1f);

        // �˺����
        foreach (var circle in circles)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                circle.transform.position,
                circleRadius
            );

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    // ���������˺�
                    hit.GetComponent<PlayerMovement>().Injured(1);
                }
            }
        }

        // �ٵȴ�1�������
        yield return new WaitForSeconds(1f);
        foreach (var circle in circles)
        {
            Destroy(circle);
        }
    }
}
