using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spanwer profile")]
public class SpawnerProfile : ScriptableObject
{
    [SerializeField] public int spawnCount = 50;
    [SerializeField] public float minDistanceToPlayer = 5;
    [SerializeField] public float maxDistanceFromPlayer = 30;
    [SerializeField] public float spawnTime = 20;
}
