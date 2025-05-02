using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRebound : MonoBehaviour
{
    // 表示玩家是否可以进行弹反的标志
    public bool canParry = false;
    // 弹反的有效时间，单位为秒
    private float parryTime = 0.5f;

    // 每帧调用一次，检查玩家是否按下 E 键，如果按下则启动弹反协程
    void Update()
    {
        // 检查玩家是否按下 E 键
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 启动弹反协程
            StartCoroutine(EnableParry());
        }
    }

    /// <summary>
    /// 协程方法，用于在一段时间内启用玩家的弹反功能
    /// </summary>
    /// <returns>协程迭代器</returns>
    IEnumerator EnableParry()
    {
        // 将弹反标志设置为 true，表示可以进行弹反
        canParry = true;
        // 等待指定的弹反有效时间
        yield return new WaitForSeconds(parryTime);
        // 将弹反标志设置为 false，表示弹反时间已过
        canParry = false;
    }

    /// <summary>
    /// 当玩家的碰撞体进入其他碰撞体时调用，检查是否可以弹反剑刃
    /// </summary>
    /// <param name="other">碰撞到的其他碰撞体</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查玩家是否可以弹反且碰到的对象标签为 "Sword"
        if (canParry && other.CompareTag("Sword"))
        {
            // 获取剑刃的 Rigidbody2D 组件
            Rigidbody2D swordRb = other.GetComponent<Rigidbody2D>();
            // 计算弹反的方向，从剑刃指向玩家
            Vector2 direction = ( other.transform.position-transform.position ).normalized;
            other.transform.localScale= new Vector3(Mathf.Abs(other.transform.localScale.x),-other.transform.localScale.y,other.transform.localScale.z);
            // 设置剑刃的速度，使其向弹反方向移动
            swordRb.velocity = direction * 10f;
            // 将剑刃的标签设置为 "ParriedSword"，表示已被弹反
            other.tag = "ParriedSword";
        }
    }
}
