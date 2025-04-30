using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float timeBetweenAttacks = 5f;
    private LaserAttack laserAttack;
    private CircleAttack circleAttack;
    private LaserBeamAttack LaserBeamAttack;

    void Start()
    {
        laserAttack = GetComponent<LaserAttack>();
        circleAttack = GetComponent<CircleAttack>();
        LaserBeamAttack = GetComponent<LaserBeamAttack>();
        StartCoroutine(AttackCycle());

    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            // 随机选择攻击方式
            if (Random.Range(0, 2) == 0)
            {
                LaserBeamAttack.StartLaserAttack(PlayerMovement.instance.transform.position);
            }
            else
            {
                LaserBeamAttack.StartLaserAttack(PlayerMovement.instance.transform.position);
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }
}
