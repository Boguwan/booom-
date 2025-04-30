using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamAttack : MonoBehaviour
{
    [Header("References")]
    public LineRenderer warningLinePrefab;
    public LineRenderer laserBeamPrefab;

    [Header("Settings")]
    public float laserDuration = 1f;
    public int laserDamage = 2;
    public float laserWidth = 0.3f;

    public void StartLaserAttack(Vector2 targetPosition)
    {
        StartCoroutine(LaserAttackSequence(targetPosition));
    }

    IEnumerator LaserAttackSequence(Vector2 targetPos)
    {
        // 生成预警线
        LineRenderer warningLine = Instantiate(warningLinePrefab, transform);
        warningLine.SetPosition(0, transform.position);
        warningLine.SetPosition(1, targetPos);

        // 等待2秒
        yield return new WaitForSeconds(2f);

        // 生成激光
        LineRenderer laser = Instantiate(laserBeamPrefab, transform);
        laser.startWidth = laserWidth;
        laser.endWidth = laserWidth;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, targetPos);

        // 碰撞检测
        StartCoroutine(DamageCheck(transform.position, targetPos));

        // 持续效果
        float timer = 0;
        while (timer < laserDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(laser.gameObject);
    }

    IEnumerator DamageCheck(Vector2 start, Vector2 end)
    {
        while (true)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerMovement>().Injured(laserDamage);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
