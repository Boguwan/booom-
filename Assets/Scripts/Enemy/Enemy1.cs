using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    public float damage;
    public float attackTime;//¹¥»÷¼ä¸ô
    public float attackTimer = 0;//¹¥»÷¶¨Ê±Æ÷

    public bool isContack = false;//ÊÇ·ñ½Ó´¥Íæ¼Ò
    public bool isCooling = false;//¹¥»÷ÀäÈ´


    private Animator animator;
    private void Start()
    {
       
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
        if (isContack && !isCooling)
        {
            Attack();
            isContack = false;
        }

        if (isCooling)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                attackTimer = 0;
                isCooling = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isContack = true;
        }
    }

    public void Move()
    {
        Vector2 direction = (Vector2)(PlayerMovement.instance.transform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);


    }
    public void Attack()
    {
        if (isCooling)
        {
            return;
        }

        PlayerMovement.instance.Injured(damage);

        isCooling = true;
        attackTimer = attackTime;
    }
}
