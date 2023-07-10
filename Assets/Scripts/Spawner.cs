using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] SpawnerProfile profile;
    [SerializeField]  EnemyEvent onEnemySpawnedEvent;
    int level;
    int spawnCount;

    private void Start()
    {
        spawnCount = profile.spawnCount;
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
            yield return new WaitForSeconds(profile.spawnTime);
        }
    }
    void SpawnEnemies()
    {
        
        Vector2 playerPos = Player.Instance.movement.lastPosition;
        for (int i = 0; i < spawnCount; i++)
        {
            var enemyPrefab = profile.enemies.GetRandomEnemy();
            Enemy enemy = Instantiate(enemyPrefab, RandomPostionAroundPlayer(playerPos, profile.maxDistanceFromPlayer), Quaternion.identity);
            enemy.Setup(level);
            onEnemySpawnedEvent?.Invoke(enemy);
        }
        level++;
        spawnCount += 5;
    }

    Vector3 RandomPostionAroundPlayer(Vector2 playerPos, float maxDistance)
    {
        //Get random position around the player and make sure its within map bounds
        Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(profile.minDistanceToPlayer, maxDistance);

        float posX = playerPos.x + offset.x;
        if(posX <= 0 || posX >= GridController.Instance.topRightCorner.x)
        {
            posX = playerPos.x - offset.x;
        }
        float posY = playerPos.y + offset.y;
        if (posY <= 0 || posY >= GridController.Instance.topRightCorner.y)
        {
            posY =  playerPos.y -offset.y ;
        }
        Vector3 randomPos = new Vector3(posX, posY, 0);
        if(GridController.Instance.IsPositionValid(randomPos))
            return randomPos;
        else
        {
            return RandomPostionAroundPlayer(playerPos, maxDistance + 1);
        }
        //TODO: Better system for handling invalid positions

    }
}


