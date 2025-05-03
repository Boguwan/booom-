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


    public float fadeDuration = 0.3f; // 淡入淡出时间

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeEffect());
        StartCoroutine(DamageOverTime());
    }

    // 淡入淡出协程
    IEnumerator FadeEffect()
    {
        // 淡入
        yield return StartCoroutine(Fade(0, 1)); // 从透明到不透明

        // 保持完全可见（总持续时间减去淡入淡出时间）
        float visibleDuration = duration - fadeDuration * 2;
        yield return new WaitForSeconds(visibleDuration);

        // 淡出
        yield return StartCoroutine(Fade(1, 0)); // 从不透明到透明
        OnFadeComplete();
    }

    // 通用渐变协程
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

        // 确保最终值准确
        color.a = endAlpha;
        spriteRenderer.color = color;
    }

     // 修改原有协程（移除自动销毁）
    IEnumerator DamageOverTime()
    {
        ApplyDamage();

        float timer = 0f;
        while (timer < duration)
        {
            // 等待间隔时间
            yield return new WaitForSeconds(damageInterval);
            ApplyDamage();
            timer += damageInterval;
        }
    }

    // 在淡出完成后销毁对象
    void OnFadeComplete()
    {
        Destroy(gameObject,2f);
    }

    

    void ApplyDamage()
    {
        // 移除已被销毁的敌人
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

