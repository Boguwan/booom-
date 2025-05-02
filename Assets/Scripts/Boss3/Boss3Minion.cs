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

    public float rotationSpeed = 30f;

    /// <summary>
    /// 协程方法，用于发射弹幕攻击，小球朝固定八个方向发射
    /// </summary>
    /// <returns>协程迭代器</returns>
    public IEnumerator FireBullets()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // 计算攻击小球的发射角度，随着时间旋转
                float angle = j * 45f + Time.time * rotationSpeed;
                // 计算攻击小球的发射方向
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                // 在 Boss 分身的位置实例化攻击小球预制体
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                // 获取攻击小球的 Rigidbody2D 组件
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                // 设置攻击小球的速度，使其向发射方向移动
                bulletRb.velocity = direction * bulletSpeed;
            }
            // 等待 0.1 秒后再发射下一轮攻击小球
            yield return new WaitForSeconds(0.1f);
        }
    }
}
