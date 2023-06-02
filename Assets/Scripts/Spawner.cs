using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] int spawnCount = 50;
    [SerializeField] float minDistanceToPlayer = 5;
    [SerializeField] float maxDistanceFromPlayer = 30;
    [SerializeField] float spawnTime = 20;
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

    IEnumerator SpawningCor()
    {
        yield return new WaitForSeconds(1);
        while(true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(spawnTime);
        }
    }
    void SpawnEnemies()
    {
        
        Vector2 playerPos = Player.Instance.transform.position;
        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = playerPos + Random.insideUnitCircle.normalized * Random.Range(minDistanceToPlayer, maxDistanceFromPlayer);
            Enemy enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
            enemy.Setup(level);
            EnemyManager.OnEnemySpawned(enemy);
        }
        level++;
        spawnCount += 5;
    }
}

