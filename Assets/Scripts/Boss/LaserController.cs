using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float checkInterval = 0.1f;
    public float damage = 1;

    void Start()
    {
        StartCoroutine(DamageCheck());
    }

    IEnumerator DamageCheck()
    {
        while (true)
        {
            Vector2 start = GetComponent<LineRenderer>().GetPosition(0);
            Vector2 end = GetComponent<LineRenderer>().GetPosition(1);

            // …‰œﬂºÏ≤‚
            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerMovement>().Injured(damage);
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
}
