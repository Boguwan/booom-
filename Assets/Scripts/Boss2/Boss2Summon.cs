using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Summon : MonoBehaviour
{
    [Header("�ٻ�����")]
    public GameObject skeletonPrefab;
    public int minSkeletons = 3;
    public int maxSkeletons = 6;
    public float summonRadius = 5f;
    public float spreadAngle = 360f;

    [Header("��Ч����")]
    public ParticleSystem summonEffect;
    public AudioClip summonSound;

    public void StartSummon()
    {
        StartCoroutine(SummonSequence());
    }

    IEnumerator SummonSequence()
    {
        // ����ǰҡ��Ч
        if (summonEffect != null)
            Instantiate(summonEffect, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(summonSound, transform.position);

        // �������ɲ���
        int skeletonCount = Random.Range(minSkeletons, maxSkeletons + 1);
        float angleStep = spreadAngle / skeletonCount;

        // ��������
        for (int i = 0; i < skeletonCount; i++)
        {
            SpawnSkeleton(i * angleStep);
            yield return new WaitForSeconds(0.2f); // ���ɼ��
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
