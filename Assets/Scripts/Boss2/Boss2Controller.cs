using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{
    public float timeBetweenAttacks = 5f;
    public FireballAttack fireballAttack;
    public Transform playerTransform;

     private LaserAttack laserAttack;
    private CircleAttack circleAttack;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        fireballAttack = GetComponent<FireballAttack>();
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            int attackType = Random.Range(0, 3); // ¸ÄÎª0-2Ëæ»ú

            switch (attackType)
            {
                case 0:
                    fireballAttack.LaunchFireballAttack(playerTransform);
                    AnimatorChange();
                    break;
                case 1:
                    fireballAttack.LaunchFireballAttack(playerTransform);
                    AnimatorChange();
                    break;
                case 2:
                    fireballAttack.LaunchFireballAttack(playerTransform);
                    AnimatorChange();
                    break;
            }

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    public void AnimatorChange()
    {
        animator.SetTrigger("isAttack");
    }
}
