using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Summon : MonoBehaviour
{
    [Header("召唤设置")]
    public GameObject skeletonPrefab;
    public int minSkeletons = 3;
    public int maxSkeletons = 6;
    public float summonRadius = 5f;
    public float spreadAngle = 360f;

    [Header("特效设置")]
    public ParticleSystem summonEffect;
    public AudioClip summonSound;

    public void StartSummon()
    {
        StartCoroutine(SummonSequence());
    }

    IEnumerator SummonSequence()
    {
        // 播放前摇特效
        if (summonEffect != null)
            Instantiate(summonEffect, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(summonSound, transform.position);

        // 计算生成参数
        int skeletonCount = Random.Range(minSkeletons, maxSkeletons + 1);
        float angleStep = spreadAngle / skeletonCount;

        // 分批生成
        for (int i = 0; i < skeletonCount; i++)
        {
            SpawnSkeleton(i * angleStep);
            yield return new WaitForSeconds(0.2f); // 生成间隔
        }
    }

    void SpawnSkeleton(float angle)
    {
        Vector2 spawnPos = CalculateSpawnPosition(angle);
        
    }

    Vector2 CalculateSpawnPosition(float angle)
    {
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        return (Vector2)transform.position + dir * summonRadius;
    }
}
