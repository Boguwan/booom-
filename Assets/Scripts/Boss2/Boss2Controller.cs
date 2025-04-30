using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    public float timeBetweenAttacks = 5f;
    public FireballAttack fireballAttack;
    public Transform playerTransform;

    void Start()
    {
        // 原有组件获取
        fireballAttack = GetComponent<FireballAttack>();
        // 需要确保玩家对象有"Player"标签
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            int attackType = Random.Range(0, 3); // 改为0-2随机

            switch (attackType)
            {
                case 0:
                    fireballAttack.LaunchFireballAttack(playerTransform);
                    break;
                case 1:
                    fireballAttack.LaunchFireballAttack(playerTransform);
                    break;
                case 2:
                    fireballAttack.LaunchFireballAttack(playerTransform);
                    break;
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }
}
