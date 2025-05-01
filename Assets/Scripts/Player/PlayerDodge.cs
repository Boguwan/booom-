using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public float dodgeDistance = 3f; // ���ܵ�С����
    public float dodgeDuration = 0.2f; // ���ܳ���ʱ��
    public float dodgeCooldown = 1f; // ������ȴʱ��
    private float lastDodgeTime;
    private bool isDodging;
    // Start is called before the first frame update
    void Start()
    {
        lastDodgeTime = Time.time - dodgeCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDodge();
    }
    void HandleDodge()
    {
        if (Input.GetKeyDown(KeyCode.Q) && CanDodge())
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
            Vector2 dodgeTarget = (Vector2)transform.position + direction * dodgeDistance;

            isDodging = true;
            lastDodgeTime = Time.time;

            // �����޵�֡
            StartCoroutine(EnableInvincibility(dodgeDuration));

            // ƽ���ƶ�������Ŀ��λ��
            StartCoroutine(MoveToDodgeTarget(dodgeTarget, dodgeDuration));
        }
    }

    bool CanDodge()
    {
        return Time.time - lastDodgeTime >= dodgeCooldown;
    }

    System.Collections.IEnumerator EnableInvincibility(float duration)
    {
        
        yield return new WaitForSeconds(duration);
        
        isDodging = false;
    }

    System.Collections.IEnumerator MoveToDodgeTarget(Vector2 target, float duration)
    {
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }
}
