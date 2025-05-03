using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    public float duration = 2f;
    public float damageInterval = 1f;
    public int damagePerTick = 5;

    private HashSet<EnemyBase> enemiesInRange = new HashSet<EnemyBase>();


    public float fadeDuration = 0.3f; // ���뵭��ʱ��

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeEffect());
        StartCoroutine(DamageOverTime());
    }

    // ���뵭��Э��
    IEnumerator FadeEffect()
    {
        // ����
        yield return StartCoroutine(Fade(0, 1)); // ��͸������͸��

        // ������ȫ�ɼ����ܳ���ʱ���ȥ���뵭��ʱ�䣩
        float visibleDuration = duration - fadeDuration * 2;
        yield return new WaitForSeconds(visibleDuration);

        // ����
        yield return StartCoroutine(Fade(1, 0)); // �Ӳ�͸����͸��
        OnFadeComplete();
    }

    // ͨ�ý���Э��
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color color = spriteRenderer.color;

        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            spriteRenderer.color = color;

            timer += Time.deltaTime;
            yield return null;
        }

        // ȷ������ֵ׼ȷ
        color.a = endAlpha;
        spriteRenderer.color = color;
    }

     // �޸�ԭ��Э�̣��Ƴ��Զ����٣�
    IEnumerator DamageOverTime()
    {
        ApplyDamage();

        float timer = 0f;
        while (timer < duration)
        {
            // �ȴ����ʱ��
            yield return new WaitForSeconds(damageInterval);
            ApplyDamage();
            timer += damageInterval;
        }
    }

    // �ڵ�����ɺ����ٶ���
    void OnFadeComplete()
    {
        Destroy(gameObject,2f);
    }

    

    void ApplyDamage()
    {
        // �Ƴ��ѱ����ٵĵ���
        enemiesInRange.RemoveWhere(enemy => enemy == null);

        foreach (var enemy in enemiesInRange.ToList())
        {
            if (enemy != null)
            {
                enemy.Injured(damagePerTick);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
        if (other.CompareTag("Fireball"))
        {
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }
}

