using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject teleportEffect;
    [SerializeField] private float teleportDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTeleport();
    }
    void HandleTeleport()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // 检测Shift键按下
        {
            
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

            // 计算瞬移目标位置（10格距离）
            Vector2 newPosition = (Vector2)transform.position + direction * teleportDistance;

            // 执行瞬移（使用物理引擎兼容方法）
            rb.MovePosition(newPosition);
            rb.velocity = Vector2.zero; // 重置速度避免惯性
            GameObject obj=  Instantiate(teleportEffect, newPosition, Quaternion.identity);
            Destroy(obj,2f);
            // 可选：添加瞬移特效/音效
            // Instantiate(teleportEffect, newPosition, Quaternion.identity);
            // AudioManager.Play("TeleportSound");
        }
    }
}
