using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAOE : MonoBehaviour
{
    public GameObject spellCirclePrefab; // ����Ԥ����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(spellCirclePrefab, transform.position, Quaternion.identity);
        }
    }
}


