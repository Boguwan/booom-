using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Minion : MonoBehaviour
{
    // ����С���Ԥ����
    public GameObject bulletPrefab;
    // ����С����ƶ��ٶ�
    public float bulletSpeed = 5f;
    // ÿ�η���Ĺ���С������
    public int bulletCount = 5;

    public float rotationSpeed = 30f;

    /// <summary>
    /// Э�̷��������ڷ��䵯Ļ������С�򳯹̶��˸�������
    /// </summary>
    /// <returns>Э�̵�����</returns>
    public IEnumerator FireBullets()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // ���㹥��С��ķ���Ƕȣ�����ʱ����ת
                float angle = j * 45f + Time.time * rotationSpeed;
                // ���㹥��С��ķ��䷽��
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                // �� Boss �����λ��ʵ��������С��Ԥ����
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                // ��ȡ����С��� Rigidbody2D ���
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                // ���ù���С����ٶȣ�ʹ�����䷽���ƶ�
                bulletRb.velocity = direction * bulletSpeed;
            }
            // �ȴ� 0.1 ����ٷ�����һ�ֹ���С��
            yield return new WaitForSeconds(0.1f);
        }
    }
}
