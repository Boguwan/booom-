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
        // 生成红圈
        List<GameObject> circles = new List<GameObject>();
        for (int i = 0; i < circleCount; i++)
        {
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * 5f;
            GameObject circle = Instantiate(redCirclePrefab, randomPos, Quaternion.identity);
            circles.Add(circle);
        }

        // 等待1秒后检测
        yield return new WaitForSeconds(1f);

        // 伤害检测
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
                    // 对玩家造成伤害
                    hit.GetComponent<PlayerMovement>().Injured(1);
                }
            }
        }

        // 再等待1秒后销毁
        yield return new WaitForSeconds(1f);
        foreach (var circle in circles)
        {
            Destroy(circle);
        }
    }
}
