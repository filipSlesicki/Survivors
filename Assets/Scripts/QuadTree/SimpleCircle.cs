using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SimpleCircle
{
    public Vector2 center;
    public float radius;

    public SimpleCircle(Vector2 pos, float radius)
    {
        center = pos;
        this.radius = radius;
    }

    public void UpdatePos(Vector2 pos)
    {
        center = pos;
    }

    public bool Contains(Vector2 point)
    {
        float toPointX = point.x - center.x;
        float toPointY = point.y - center.y;

        return toPointX * toPointX + toPointY * toPointY <= radius * radius;
    }
}
