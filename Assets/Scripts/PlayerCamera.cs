using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smoothTime = .2f;
    Vector3 vel;

    void LateUpdate()
    {
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, player.position, ref vel, smoothTime);
        targetPos.z = -10;
        transform.position = targetPos;
    }
}
