using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField]GameObject expPickupPrefab;
    //TODO: Add more loot

    /// <summary>
    /// Spawns exp on enemy death. Called from event
    /// </summary>
    /// <param name="enemyDeathInfo"></param>
    public void SpawnLoot(DeathInfo enemyDeathInfo)
    {
        Instantiate(expPickupPrefab, enemyDeathInfo.killed.transform.position, Quaternion.identity);
    }
}
