using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : Movement
{
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = rb.position;
    }

    public override void SetDirection(Vector2 dir)
    {
        rb.velocity = dir * moveSpeed;
        lastPosition = rb.position;
    }
    public override void Move(Vector2 direction, float dt)
    {
        lastPosition += moveSpeed * dt * direction;
        rb.MovePosition(lastPosition);
    }
}
