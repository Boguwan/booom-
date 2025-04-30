using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float timer;
    private Vector2 targetPos;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void Init(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        targetPos = end;
        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        float blinkSpeed = 5f;
        while (timer < 2f)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1);
            lineRenderer.startColor = new Color(1, 0, 0, alpha);
            lineRenderer.endColor = new Color(1, 0, 0, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
