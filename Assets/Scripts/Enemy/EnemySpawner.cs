using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab; // ����Prefab
    public float spawnInterval = 5f; // ���ɼ��
    public int enemiesPerWave = 3; // ÿ����������

    private BoxCollider2D spawnArea; // ����������ײ��

    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 spawnPos = GetRandomPosition();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector2 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;
        return new Vector2(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y)
        );
    }
}
