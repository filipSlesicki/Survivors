using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPPickup : MonoBehaviour
{
    [SerializeField] int expValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerLevel.Instance.AddExp(expValue);
            Destroy(gameObject);
        }
    }
}
