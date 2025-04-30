using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 5f;
    public float curveIntensity = 2f; // 曲线弯曲程度
    public int fireballCount = 3;     // 每次发射数量

    public float spreadAngle = 30f; // 两条火球之间的夹角

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

            // 生成左右两个控制点
            (Vector2 leftControl, Vector2 rightControl) = CalculateDualControlPoints(
                startPoint,
                playerPos
            );

            // 同时创建两个火球
            CreateFireball(startPoint, leftControl, playerPos);
            CreateFireball(startPoint, rightControl, playerPos);

            yield return new WaitForSeconds(0.3f);
        }
        
    }

    (Vector2, Vector2) CalculateDualControlPoints(Vector2 start, Vector2 end)
    {
        Vector2 midPoint = Vector2.Lerp(start, end, 0.5f);
        Vector2 baseDirection = (end - start).normalized;

        // 计算左右偏移方向
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

            // 计算当前位置和切线方向
            Vector2 currentPos = CalculateBezierPoint(t, p0, p1, p2);
            Vector2 tangent = CalculateBezierDerivative(t, p0, p1, p2);

            // 更新位置和旋转
            fireball.position = currentPos;
            if (tangent != Vector2.zero)
            {
                float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
                fireball.rotation = Quaternion.Euler(0, 0, angle ); // -90度修正面朝方向
            }

            t += Time.deltaTime * fireballSpeed;
            yield return null;
        }

        if (fireball != null)
            Destroy(fireball.gameObject);
    }

    Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        // 二次贝塞尔曲线公式
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
    Vector2 CalculateBezierDerivative(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
    }
}
