using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// List of near enemies. Set in <see cref="EnemyDetector"/> on player
    /// </summary>
    List<Enemy> nearbyEnemies = new List<Enemy>();
    /// <summary>
    /// List of near enemies. Set in <see cref="EnemyDetector"/> on player
    /// </summary>
    List<Enemy> farEnemies = new List<Enemy>();

    List<Enemy> nearbyToRemove = new List<Enemy>();
    [Tooltip("Only update enemies every n frames")]
    [SerializeField] int skipUpdatesFar = 3;
    /// <summary>
    /// Frames passed since last far enemies update
    /// </summary>
    int frameCount;

    public static EnemyManager Instance;

    private void Awake()
    {
        Instance = this;
    }

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
            for (int i = count - 1; i >= 0; i--)
            {
                farEnemies[i].SimpleTick(dt);
            }
        }
        else //Dont update near enemies in the same frame as far enemies
        {
            int count = nearbyEnemies.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                nearbyEnemies[i].Tick(dt);
            }
        }
        UpdateEnemyLists();
    }

    public Enemy GetClosestEnemyFromPoint(Vector3 from)
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
    /// <summary>
    /// Handles enemy spawn event raised by <see cref="Spawner"/>
    /// </summary>
    /// <param name="enemy"></param>
    public void OnEnemySpawned(Enemy enemy)
    {
        farEnemies.Add(enemy);
    }

    public void AddNearbyEnemy(Enemy enemy)
    {
        nearbyEnemies.Add(enemy);
        farEnemies.Remove(enemy);
    }

    void RemoveNearbyEnemy(Enemy enemy)
    {
        nearbyEnemies.Remove(enemy);
        if (enemy != null && enemy.currentHealth > 0)
        {
            farEnemies.Add(enemy);
        }
    }

    public void OnEnemyLeftRange(Enemy enemy)
    {
        nearbyToRemove.Add(enemy);
    }


    void UpdateEnemyLists()
    {
        foreach (var enemy in nearbyToRemove)
        {
            RemoveNearbyEnemy(enemy);
        }
        nearbyToRemove.Clear();
    }
    public void OnEnemyDied(DeathInfo enemyDeathInfo)
    {
        nearbyEnemies.Remove(enemyDeathInfo.killed as Enemy);
        farEnemies.Remove(enemyDeathInfo.killed as Enemy);
    }
}
