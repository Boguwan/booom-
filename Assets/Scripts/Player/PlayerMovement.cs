using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float hp;
    public bool isDead;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform tf;
    private Animator animator;
    private float horizontalInput;
    private float verticalInput;

    public static PlayerMovement instance;

    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        tf = GetComponent<Transform>();
    }

    void Update()
    {
        // 获取输入
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // 处理转向和动画
        UpdateDirection();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // 物理移动
        rb.velocity = new Vector2(horizontalInput, verticalInput).normalized*moveSpeed;
        
    }

    void UpdateDirection()
    {
        if (horizontalInput > 0)
        {
            transform.localScale=new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);  
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void UpdateAnimation()
    {
        bool isMoving = (horizontalInput != 0||verticalInput!=0);
        animator.SetBool("isWalking", isMoving);
    }

    public void Injured(float damage)
    {
        if(isDead) return;  

        
        if(hp -damage< 0)
        {
            hp = 0;
            
            Dead();
        }
        else
        {
            animator.SetTrigger("isInjured");
            hp -= damage;
        }
    }

    public void Dead()
    {
        isDead = true;
    }
    void AnimationChangeInjured()
    {
        animator.SetTrigger("isInjured");
    }
}
