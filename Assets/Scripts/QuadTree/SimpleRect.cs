using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SimpleRect
{
    public Vector2 center;
    public float halfWidth;
    public float halfHeight;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    public SimpleRect(Vector2 pos, float width, float height)
    {
        center = pos;
        this.halfWidth = width/2;
        this.halfHeight = height/2;
        minX = center.x - halfWidth;
        maxX = center.x + halfWidth;
        minY = center.y - halfHeight;
        maxY = center.y + halfHeight;
    }

    public void UpdatePos(Vector2 pos)
    {
        center = pos;
        minX = center.x - halfWidth;
        maxX = center.x + halfWidth;
        minY = center.y - halfHeight;
        maxY = center.y + halfHeight;
    }

    public bool Contains(Vector2 point)
    {
        return point.x >= minX && point.x <= maxX &&
               point.y >= minY && point.y <= maxY;
    }

    public bool OverlapsRect(SimpleRect other)
    {
        return minX <= other.maxX && maxX >= other.minX &&
            minY <= other.maxY && maxY >= other.minY;
    }

    public bool OverlapsCircle(SimpleCircle circle)
    {
        // Calculate the closest point on the rectangle to the circle center
        float closestX = circle.center.x;
        float closestY = circle.center.y;

        if (circle.center.x < center.x - halfWidth)
            closestX = center.x - halfWidth;
        else if (circle.center.x > center.x + halfWidth)
            closestX = center.x + halfWidth;

        if (circle.center.y < center.y - halfHeight)
            closestY = center.y - halfHeight;
        else if (circle.center.y > center.y + halfHeight)
            closestY = center.y + halfHeight;

        // Calculate the squared distance between the closest point and the circle center
        float deltaX = closestX - circle.center.x;
        float deltaY = closestY - circle.center.y;
        float distanceSq = deltaX * deltaX + deltaY * deltaY;

        // Compare squared distances with the circle's squared radius
        float radiusSq = circle.radius * circle.radius;
        return distanceSq <= radiusSq;
    }
}
