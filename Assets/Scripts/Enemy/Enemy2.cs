using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    public float moveSpeed = 3f;
    public float attackRange = 5f;
    public float fireballSpeed = 8f;
    public float attackCooling = 3f;
    

   
    public GameObject fireballPrefab;
    public Transform firePoint;
    private Animator animator;

    public Transform player;
    private bool isAttacking = false;
    private Coroutine attackCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 面向玩家
        
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            animator.SetBool("isAttack", false);
            // 移动朝向玩家
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            
            

            if (isAttacking)
            {
                StopCoroutine(attackCoroutine);
                isAttacking = false;
            }
        }
        else if (!isAttacking)
        {
            animator.SetBool("isAttack",true);
            attackCoroutine = StartCoroutine(AttackRoutine());
            isAttacking = true;
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(attackCooling);
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        if (fireballPrefab && firePoint && player)
        {
            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - firePoint.position).normalized;

            fireball.GetComponent<Rigidbody2D>().velocity = direction * fireballSpeed;

            // 设置火球旋转角度
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Destroy(fireball, 3f); // 3秒后自动销毁
        }
    }

    void FaceTarget()
    {
        if (PlayerMovement.instance.transform.position.x - transform.position.x >= 0.1)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (PlayerMovement.instance.transform.position.x - transform.position.x < 0.1)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    
}
