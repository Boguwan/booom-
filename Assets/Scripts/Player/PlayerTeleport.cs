using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject teleportEffect;
    [SerializeField] private float teleportDistance = 10f;

    public float teleportCooldown = 5f;
    private float lastTeleportTime=0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastTeleportTime = Time.time - teleportCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTeleport();
        
    }

    bool CanTeleport()
    {
        // �����ȴʱ�� + �ɹ���״̬
        return Time.time - lastTeleportTime >= teleportCooldown;
    }
    void HandleTeleport()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&&CanTeleport()) // ���Shift������
        {
            
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

            // ����˲��Ŀ��λ�ã�
            Vector2 newPosition = (Vector2)transform.position + direction * teleportDistance;

            // ִ��˲��
            rb.MovePosition(newPosition);
            rb.velocity = Vector2.zero; // �����ٶ�
            GameObject obj=  Instantiate(teleportEffect, newPosition, Quaternion.identity);
            Destroy(obj,2f);

            lastTeleportTime = Time.time;
        }
    }
}
