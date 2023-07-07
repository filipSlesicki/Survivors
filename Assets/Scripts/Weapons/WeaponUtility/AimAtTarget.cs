using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtTarget : MonoBehaviour
{
    public void Aim(Vector3 targetPos)
    {
        Vector3 toTarget = targetPos - transform.position;
        transform.right = toTarget;
    }
}
