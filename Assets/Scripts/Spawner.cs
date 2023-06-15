using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] EnemySpawnChance[] enemies;
    [SerializeField] SpawnerProfile profile;
    int level;

    private void Start()
    {
        StartCoroutine(SpawningCor());
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnEnemies();
        }
    }

    Enemy GetRandomEnemy()
    {
        float random = Random.Range(0, 100);
        foreach (var enemy in enemies)
        {
            if(random >= enemy.chanceMin && random < enemy.chanceMax)
            {
                return enemy.prefab;
            }
        }
        return enemies[0].prefab;
    }

    IEnumerator SpawningCor()
    {
        yield return new WaitForSeconds(1);
        while(true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(profile.spawnTime);
        }
    }
    void SpawnEnemies()
    {
        
        Vector2 playerPos = Player.Instance.transform.position;
        for (int i = 0; i < profile.spawnCount; i++)
        {
            Vector2 randomPos = playerPos + Random.insideUnitCircle.normalized * 
                Random.Range(profile.minDistanceToPlayer, profile.maxDistanceFromPlayer);
            Enemy enemy = Instantiate(GetRandomEnemy(), randomPos, Quaternion.identity);
            enemy.Setup(level);
            EnemyManager.OnEnemySpawned(enemy);
        }
        level++;
        profile.spawnCount += 5;
    }
}

[System.Serializable]
public class EnemySpawnChance
{
    public Enemy prefab;
    public float chanceMin;
    public float chanceMax;
}


