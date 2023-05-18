using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{

    [SerializeField] FlameThrower thrower;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity target;
        if(collision.TryGetComponent(out target))
        {
            thrower.AddTarget(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Entity target;
        if (collision.TryGetComponent(out target))
        {
            thrower.RemoveTarget(target);
        }

    }
}
