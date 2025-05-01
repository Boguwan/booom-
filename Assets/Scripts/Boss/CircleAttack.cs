using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : MonoBehaviour
{
    [Header("��������")]
    public int maxCircles = 50;        // ����Ȧ����
    public float minRadius = 2f;       // ��С���ɰ뾶
    public float maxRadius = 20f;      // ��ͼ���뾶
    public float densityFactor = 1.5f; // �ܶ�����ϵ����>1ʱ�������裩

    [Header("��Ȧ����")]
    public GameObject redCirclePrefab;
    public float circleRadius = 1f;

    public void StartCircleAttack()
    {
        StartCoroutine(GenerateDensityCircles());
    }

    IEnumerator GenerateDensityCircles()
    {
        List<Vector2> generatedPositions = new List<Vector2>();

        // �����ܶ���������λ��
        for (int i = 0; i < maxCircles; i++)
        {
            Vector2 pos = GenerateDensityPoint();

            // �����ص����
            if (!IsOverlapping(pos, generatedPositions))
            {
                InstantiateCircle(pos);
                generatedPositions.Add(pos);
            }

            yield return null; // ��֡����
        }
    }

    Vector2 GenerateDensityPoint()
    {
        // ʹ��ָ���ֲ������ܶ�
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
        // ����Ӵ�С����Ч��
        circle.transform.localScale = Vector3.one *
            Mathf.Lerp(0.8f, 1.2f,
                Vector2.Distance(position, transform.position) / maxRadius);
    }
}
