using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    public int laserCount = 12;
    public float laserLength = 5f;
    public float rotationSpeed = 30f;
    public float attackDuration = 3f;
    public LineRenderer laserPrefab;

    public void StartLaserAttack()
    {
        StartCoroutine(LaserAttackRoutine());
    }

    IEnumerator LaserAttackRoutine()
    {
        List<LineRenderer> lasers = new List<LineRenderer>();
        List<float> angles = new List<float>();

        // 创建激光
        for (int i = 0; i < laserCount; i++)
        {
            LineRenderer laser = Instantiate(laserPrefab, transform);
            lasers.Add(laser);
            angles.Add(i * 360f / laserCount);
        }

        // 旋转激光
        float timer = 0;
        while (timer < attackDuration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < lasers.Count; i++)
            {
                angles[i] += rotationSpeed * Time.deltaTime;
                Vector2 direction = Quaternion.Euler(0, 0, angles[i]) * Vector2.right;

                lasers[i].SetPosition(0, transform.position);
                lasers[i].SetPosition(1, (Vector2)transform.position + direction * laserLength);
            }

            yield return null;
        }

        // 清除激光
        foreach (var laser in lasers)
        {
            Destroy(laser.gameObject);
        }
    }
}
