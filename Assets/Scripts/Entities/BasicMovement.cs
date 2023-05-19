using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : Movement
{
    Transform myTranform;
    Vector3 lastPosition;
    private void Start()
    {
        myTranform = transform;
        lastPosition = myTranform.position;
    }
    public override void Move(Vector3 direction)
    {
        lastPosition += moveSpeed * Time.deltaTime * direction;
        myTranform.position = lastPosition;
    }
}
