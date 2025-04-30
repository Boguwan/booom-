using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    public float damage;

    void Update()
    {
        // 直接位置更新（每帧移动）
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void Init(Vector2 dir, float spd, float lifetime)
    {
        direction = dir;
        speed = spd;
        Destroy(gameObject, lifetime); // 自动销毁
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Injured(damage);
            }
            Destroy(gameObject);
        }
        
        
    }
}
