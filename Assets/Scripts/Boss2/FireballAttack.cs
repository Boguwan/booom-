using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 5f;
    public float curveIntensity = 2f; // ���������̶�
    public int fireballCount = 3;     // ÿ�η�������

    public float spreadAngle = 30f; // ��������֮��ļн�

    public void LaunchFireballAttack(Transform player)
    {
        StartCoroutine(DualFireballAttackRoutine(player));
    }

    IEnumerator DualFireballAttackRoutine(Transform player)
    {
        for (int i = 0; i < fireballCount; i++)
        {
            Vector2 startPoint = transform.position;
            Vector2 playerPos = player.position;

            // ���������������Ƶ�
            (Vector2 leftControl, Vector2 rightControl) = CalculateDualControlPoints(
                startPoint,
                playerPos
            );

            // ͬʱ������������
            CreateFireball(startPoint, leftControl, playerPos);
            CreateFireball(startPoint, rightControl, playerPos);

            yield return new WaitForSeconds(0.3f);
        }
        
    }

    (Vector2, Vector2) CalculateDualControlPoints(Vector2 start, Vector2 end)
    {
        Vector2 midPoint = Vector2.Lerp(start, end, 0.5f);
        Vector2 baseDirection = (end - start).normalized;

        // ��������ƫ�Ʒ���
        Vector2 leftPerpendicular = new Vector2(-baseDirection.y, baseDirection.x);
        Vector2 rightPerpendicular = new Vector2(baseDirection.y, -baseDirection.x);

        return (
            midPoint + leftPerpendicular * curveIntensity,
            midPoint + rightPerpendicular * curveIntensity
        );
    }

    void CreateFireball(Vector2 start, Vector2 control, Vector2 end)
    {
        GameObject fireball = Instantiate(
            fireballPrefab,
            start,
            Quaternion.identity
        );

        StartCoroutine(MoveAlongCurve(
            fireball.transform,
            start,
            control,
            end
        ));
    }


    IEnumerator MoveAlongCurve(Transform fireball, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float t = 0;

        while (t < 1)
        {
            if (fireball == null || !fireball.gameObject.activeSelf)
                yield break;

            // ���㵱ǰλ�ú����߷���
            Vector2 currentPos = CalculateBezierPoint(t, p0, p1, p2);
            Vector2 tangent = CalculateBezierDerivative(t, p0, p1, p2);

            // ����λ�ú���ת
            fireball.position = currentPos;
            if (tangent != Vector2.zero)
            {
                float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
                fireball.rotation = Quaternion.Euler(0, 0, angle ); // -90�������泯����
            }

            t += Time.deltaTime * fireballSpeed;
            yield return null;
        }

        if (fireball != null)
            Destroy(fireball.gameObject);
    }

    Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        // ���α��������߹�ʽ
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
    Vector2 CalculateBezierDerivative(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
    }
}
