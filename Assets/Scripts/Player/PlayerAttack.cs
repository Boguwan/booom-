using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public float angleBetweenFireballs = 20f;
    public float fireballLifetime = 2f;

    private Animator animator;

    [SerializeField] private float attackCooldown = 0.5f; // �����������������
    private float lastAttackTime; // ��������¼�ϴι���ʱ��
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            animator.SetTrigger("isAttack");
            ShootFireballs();
            lastAttackTime = Time.time;
        }
    }
    bool CanAttack()
    {
        // �����ȴʱ�� + �ɹ���״̬������չ����״̬�жϣ�
        return Time.time - lastAttackTime >= attackCooldown;
    }

    void ShootFireballs()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        float[] angles = { -angleBetweenFireballs / 2, 0, angleBetweenFireballs / 2 };

        foreach (float angle in angles)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 finalDirection = rotation * direction;

            // ʵ����������Rigidbody��
            GameObject fireball = Instantiate(fireballPrefab,
                transform.position,
                Quaternion.FromToRotation(Vector2.right, finalDirection));

            // ֱ�ӿ���Transform�ƶ��������ű���
            FireballMovement fm = fireball.AddComponent<FireballMovement>();
            fm.Init(finalDirection, fireballSpeed, fireballLifetime);
        }
    }
    void AnimationChange()
    {
        animator.SetTrigger("isAttack");
    }
}

