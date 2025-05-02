using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    // 剑刃造成的伤害值
    public float damage = 10f;

    /// <summary>
    /// 当剑刃的碰撞体进入其他碰撞体时调用，处理碰撞逻辑
    /// </summary>
    /// <param name="other">碰撞到的其他碰撞体</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰到的对象标签为 "Player"
        if (other.CompareTag("Player")&&other.GetComponent<PlayerRebound>().canParry==false)
        {
            other.GetComponent<PlayerMovement>().Injured(damage);
            // 销毁剑刃对象
            Destroy(gameObject);
        }
        // 检查碰到的对象标签为 "Boss" 且剑刃已被弹反
        else if (other.CompareTag("Boss") && tag == "ParriedSword")
        {
            // 获取 Boss 的 BossController 组件
            Boss3Controller boss = other.GetComponent<Boss3Controller>();
            // 检查 Boss 组件是否存在
            if (boss != null)
            {
                // 调用 Boss 的受伤害方法
                boss.TakeDamage(damage);
            }
            // 销毁剑刃对象
            Destroy(gameObject);
        }
    }
}
