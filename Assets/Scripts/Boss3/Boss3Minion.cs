using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Minion : MonoBehaviour
{
    // 攻击小球的预制体
    public GameObject bulletPrefab;
    // 攻击小球的移动速度
    public float bulletSpeed = 5f;
    // 每次发射的攻击小球数量
    public int bulletCount = 5;
    // 攻击小球发射的旋转速度
    public float rotationSpeed = 30f;

    /// <summary>
    /// 当 Boss 分身被销毁时调用，启动弹幕攻击协程
    /// </summary>
    void OnDestroy()
    {
        // 启动弹幕攻击协程
        StartCoroutine(FireBullets());
    }

    /// <summary>
    /// 协程方法，用于发射弹幕攻击
    /// </summary>
    /// <returns>协程迭代器</returns>
    IEnumerator FireBullets()
    {
        
        for (int i = 0; i < bulletCount; i++)
        {
            // 向八个方向发射攻击小球
            for (int j = 0; j < 8; j++)
            {
               
                float angle = j * 45f + Time.time * rotationSpeed;
               
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
               
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                
                bulletRb.velocity = direction * bulletSpeed;
            }
            // 等待 0.1 秒后再发射下一轮攻击小球
            yield return new WaitForSeconds(0.1f);
        }
    }
}
