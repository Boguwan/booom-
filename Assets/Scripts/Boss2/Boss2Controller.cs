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
        // ԭ�������ȡ
        fireballAttack = GetComponent<FireballAttack>();
        // ��Ҫȷ����Ҷ�����"Player"��ǩ
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            int attackType = Random.Range(0, 3); // ��Ϊ0-2���

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
