using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Update()
    {
        foreach (Enemy enemy in allEnemies)
        {
            enemy.Tick();
        }
    }

    public static Enemy GetClosestEnemy()
    {
        float minDistance = float.MaxValue;
        Enemy closest = null;
        foreach (Enemy enemy in allEnemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, Player.Instance.transform.position);
            if (distance < minDistance)
            {
                closest = enemy;
                minDistance = distance;
            }
        }
        return closest;
    }
}
