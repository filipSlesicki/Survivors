using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : Movement
{
    public override void Move(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
