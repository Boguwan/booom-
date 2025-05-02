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
    // ����С�������ת�ٶ�
    public float rotationSpeed = 30f;

    /// <summary>
    /// �� Boss ��������ʱ���ã�������Ļ����Э��
    /// </summary>
    void OnDestroy()
    {
        // ������Ļ����Э��
        StartCoroutine(FireBullets());
    }

    /// <summary>
    /// Э�̷��������ڷ��䵯Ļ����
    /// </summary>
    /// <returns>Э�̵�����</returns>
    IEnumerator FireBullets()
    {
        
        for (int i = 0; i < bulletCount; i++)
        {
            // ��˸������乥��С��
            for (int j = 0; j < 8; j++)
            {
               
                float angle = j * 45f + Time.time * rotationSpeed;
               
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
               
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                
                bulletRb.velocity = direction * bulletSpeed;
            }
            // �ȴ� 0.1 ����ٷ�����һ�ֹ���С��
            yield return new WaitForSeconds(0.1f);
        }
    }
}
