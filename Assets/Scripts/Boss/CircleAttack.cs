using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    [Header("生成设置")]
    public int maxCircles = 50;        // 最大红圈数量
    public float minRadius = 2f;       // 最小生成半径
    public float maxRadius = 20f;      // 地图最大半径
    public float densityFactor = 1.5f; // 密度增长系数（>1时外密内疏）

    [Header("红圈设置")]
    public GameObject redCirclePrefab;
    public float circleRadius = 1f;

    public void StartCircleAttack()
    {
        StartCoroutine(GenerateDensityCircles());
    }

    IEnumerator GenerateDensityCircles()
    {
        List<Vector2> generatedPositions = new List<Vector2>();

        // 根据密度曲线生成位置
        for (int i = 0; i < maxCircles; i++)
        {
            Vector2 pos = GenerateDensityPoint();

            // 避免重叠检测
            if (!IsOverlapping(pos, generatedPositions))
            {
                InstantiateCircle(pos);
                generatedPositions.Add(pos);
            }

            yield return null; // 分帧生成
        }
    }

    Vector2 GenerateDensityPoint()
    {
        // 使用指数分布控制密度
        float t = Mathf.Pow(Random.value, 1 / densityFactor);
        float r = Mathf.Lerp(minRadius, maxRadius, t);
        float angle = Random.Range(0, 360f);

        return (Vector2)transform.position +
               new Vector2(
                   r * Mathf.Cos(angle * Mathf.Deg2Rad),
                   r * Mathf.Sin(angle * Mathf.Deg2Rad)
               );
    }

    bool IsOverlapping(Vector2 newPos, List<Vector2> existingPos)
    {
        foreach (Vector2 pos in existingPos)
        {
            if (Vector2.Distance(newPos, pos) < circleRadius * 2)
                return true;
        }
        return false;
    }

    void InstantiateCircle(Vector2 position)
    {
        GameObject circle = Instantiate(
            redCirclePrefab,
            position,
            Quaternion.identity
        );
        // 可添加大小渐变效果
        circle.transform.localScale = Vector3.one *
            Mathf.Lerp(0.8f, 1.2f,
                Vector2.Distance(position, transform.position) / maxRadius);
    }
}
