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
                laserAttack.StartLaserAttack();
            }
            else
            {
                circleAttack.StartCircleAttack();
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }
}
