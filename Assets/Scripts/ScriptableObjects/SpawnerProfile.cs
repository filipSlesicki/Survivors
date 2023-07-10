using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spanwer profile")]
public class SpawnerProfile : ScriptableObject
{
    public int spawnCount = 50;
    public float minDistanceToPlayer = 5;
    public float maxDistanceFromPlayer = 30;
    public float spawnTime = 20;
    public RandomEnemyChance enemies;
}
