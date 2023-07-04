using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    static List<Enemy> nearbyEnemies = new List<Enemy>();
    static List<Enemy> farEnemies = new List<Enemy>();

    [SerializeField] int skipUpdatesFar = 3;
    int frameCount;

    private void OnDisable()
    {

        nearbyEnemies.Clear();
        farEnemies.Clear();
    }


    private void FixedUpdate()
    {
        frameCount++;
        float dt = Time.deltaTime;

        if (frameCount % skipUpdatesFar == 0) //Update far enemies every n frames
        {
            int count = farEnemies.Count;
            for (int i = 0; i < count; i++)
            {
                farEnemies[i].SimpleTick(dt);
            }
        }
        else //Dont update close enemies every n frames
        {
            int count = nearbyEnemies.Count;
            for (int i = 0; i < count; i++)
            {
                nearbyEnemies[i].Tick(dt);
            }
        }
    }

    public static Enemy GetClosestEnemyFromPoint(Vector3 from)
    {
        float minDistance = float.MaxValue;
        Enemy closest = null;
        foreach (Enemy enemy in nearbyEnemies)
        {
            float distance = Vector2.Distance(enemy.movement.lastPosition, from);
            if (distance < minDistance)
            {
                closest = enemy;
                minDistance = distance;
            }
        }
        return closest;
    }

    public static void OnEnemySpawned(Enemy enemy)
    {
        farEnemies.Add(enemy);
    }

    public static void AddNearbyEnemy(Enemy enemy)
    {
        nearbyEnemies.Add(enemy);
        farEnemies.Remove(enemy);
    }

    public static void RemoveNearbyEnemy(Enemy enemy)
    {
        nearbyEnemies.Remove(enemy);
        if (enemy != null && enemy.currentHealth > 0)
        {
            farEnemies.Add(enemy);
        }
    }

    public void OnEnemyDied(DeathInfo enemyDeathInfo)
    {
        nearbyEnemies.Remove(enemyDeathInfo.killed as Enemy);
        farEnemies.Remove(enemyDeathInfo.killed as Enemy);
    }
}
