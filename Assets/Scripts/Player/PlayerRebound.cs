using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRebound : MonoBehaviour
{
    // ��ʾ����Ƿ���Խ��е����ı�־
    public bool canParry = false;
    // ��������Чʱ�䣬��λΪ��
    private float parryTime = 0.5f;

    // ÿ֡����һ�Σ��������Ƿ��� E ���������������������Э��
    void Update()
    {
        // �������Ƿ��� E ��
        if (Input.GetKeyDown(KeyCode.E))
        {
            // ��������Э��
            StartCoroutine(EnableParry());
        }
    }

    /// <summary>
    /// Э�̷�����������һ��ʱ����������ҵĵ�������
    /// </summary>
    /// <returns>Э�̵�����</returns>
    IEnumerator EnableParry()
    {
        // ��������־����Ϊ true����ʾ���Խ��е���
        canParry = true;
        // �ȴ�ָ���ĵ�����Чʱ��
        yield return new WaitForSeconds(parryTime);
        // ��������־����Ϊ false����ʾ����ʱ���ѹ�
        canParry = false;
    }

    /// <summary>
    /// ����ҵ���ײ�����������ײ��ʱ���ã�����Ƿ���Ե�������
    /// </summary>
    /// <param name="other">��ײ����������ײ��</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // �������Ƿ���Ե����������Ķ����ǩΪ "Sword"
        if (canParry && other.CompareTag("Sword"))
        {
            // ��ȡ���е� Rigidbody2D ���
            Rigidbody2D swordRb = other.GetComponent<Rigidbody2D>();
            // ���㵯���ķ��򣬴ӽ���ָ�����
            Vector2 direction = ( other.transform.position-transform.position ).normalized;
            other.transform.localScale= new Vector3(Mathf.Abs(other.transform.localScale.x),-other.transform.localScale.y,other.transform.localScale.z);
            // ���ý��е��ٶȣ�ʹ���򵯷������ƶ�
            swordRb.velocity = direction * 10f;
            // �����еı�ǩ����Ϊ "ParriedSword"����ʾ�ѱ�����
            other.tag = "ParriedSword";
        }
    }
}
