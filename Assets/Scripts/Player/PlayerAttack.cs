using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public float angleBetweenFireballs = 20f;
    public float fireballLifetime = 2f;
    public int fireballCount = 3;

    private Animator animator;

    [SerializeField] private float attackCooldown = 0.5f; // 新增：攻击间隔参数
    private float lastAttackTime; // 新增：记录上次攻击时间
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
        // 检查冷却时间 + 可攻击状态（可扩展动画状态判断）
        return Time.time - lastAttackTime >= attackCooldown;
    }

    void ShootFireballs()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

        float startAngle = -(fireballCount - 1) * angleBetweenFireballs / 2;

        for (int i = 0; i < fireballCount; i++)
        {
            float currentAngle = startAngle + i * angleBetweenFireballs;
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector2 rotatedDirection = rotation * dir;

            GameObject fb = Instantiate(fireballPrefab,
                transform.position,
                Quaternion.Euler(0, 0, currentAngle + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));

            fb.AddComponent<FireballMovement>().Init(
                rotatedDirection,
                fireballSpeed,
                fireballLifetime
            );
        }
    }
    void AnimationChange()
    {
        animator.SetTrigger("isAttack");
    }
}

