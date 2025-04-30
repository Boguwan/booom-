using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Rigidbody2D rb;

    public float knockbackForce = 5f;  // 击退力度
    public float hp;
    public float speed;
    

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        TurnRound();
        
    }

    

    public void TurnRound()
    {
        //玩家在怪物右边
        if (PlayerMovement.instance.transform.position.x - transform.position.x >= 0.1)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if(PlayerMovement.instance.transform.position.x - transform.position.x < 0.1)
        {
            transform.localScale=new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);   
        }
    }

    

    public void Injured(float damage)
    {
        if (hp - damage < 0)
        {
            hp = 0;

            Dead();
        }
        else
        {
            hp -= damage;
        }
        Vector2 direction = (transform.position - PlayerMovement.instance.transform.position).normalized;
        rb.velocity = direction * knockbackForce;
    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}
