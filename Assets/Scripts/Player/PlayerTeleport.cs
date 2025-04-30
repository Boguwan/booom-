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
        if (Input.GetKeyDown(KeyCode.LeftShift)) // ���Shift������
        {
            
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

            // ����˲��Ŀ��λ�ã�10����룩
            Vector2 newPosition = (Vector2)transform.position + direction * teleportDistance;

            // ִ��˲�ƣ�ʹ������������ݷ�����
            rb.MovePosition(newPosition);
            rb.velocity = Vector2.zero; // �����ٶȱ������
            GameObject obj=  Instantiate(teleportEffect, newPosition, Quaternion.identity);
            Destroy(obj,2f);
            // ��ѡ�����˲����Ч/��Ч
            // Instantiate(teleportEffect, newPosition, Quaternion.identity);
            // AudioManager.Play("TeleportSound");
        }
    }
}
