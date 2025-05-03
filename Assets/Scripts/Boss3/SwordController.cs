using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    // ������ɵ��˺�ֵ
    public float damage = 10f;

    /// <summary>
    /// �����е���ײ�����������ײ��ʱ���ã�������ײ�߼�
    /// </summary>
    /// <param name="other">��ײ����������ײ��</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // ��������Ķ����ǩΪ "Player"
        if (other.CompareTag("Player")&&other.GetComponent<PlayerRebound>().canParry==false)
        {
            other.GetComponent<PlayerMovement>().Injured(damage);
            // ���ٽ��ж���
            Destroy(gameObject);
        }
        // ��������Ķ����ǩΪ "Boss" �ҽ����ѱ�����
        else if (other.CompareTag("Boss") && tag == "ParriedSword")
        {
            // ��ȡ Boss �� BossController ���
            Boss3Controller boss = other.GetComponent<Boss3Controller>();
            // ��� Boss ����Ƿ����
            if (boss != null)
            {
                // ���� Boss �����˺�����
                boss.TakeDamage(damage);
            }
            // ���ٽ��ж���
            Destroy(gameObject);
        }
    }
}
