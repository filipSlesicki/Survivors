using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Update()
    {
        int count = allEnemies.Count;
        for (int i = 0; i < count; i++)
        {
            allEnemies[i].Tick();
        }

    }

    public static Enemy GetClosestEnemyFromPoint(Vector3 from)
    {
        float minDistance = float.MaxValue;
        Enemy closest = null;
        foreach (Enemy enemy in allEnemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, from);
            if (distance < minDistance)
            {
                closest = enemy;
                minDistance = distance;
            }
        }
        return closest;
    }
}
