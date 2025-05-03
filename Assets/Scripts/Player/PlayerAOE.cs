using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAOE : MonoBehaviour
{
    public GameObject spellCirclePrefab;

    public float magicCooldown = 5f;
    private float lastMagicTime=0f;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        lastMagicTime = Time.time-magicCooldown;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&CanMagic())
        {
            animator.SetTrigger("isAttack");
            Instantiate(spellCirclePrefab, transform.position, Quaternion.identity);
            lastMagicTime = Time.time;
        }
        
    }

    bool CanMagic()
    {
        return Time.time -lastMagicTime > magicCooldown;
    }
}


