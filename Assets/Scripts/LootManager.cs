using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField]GameObject expPickup;

    public void SpawnLoot(DeathInfo enemyDeathInfo)
    {
        Instantiate(expPickup, enemyDeathInfo.killed.transform.position, Quaternion.identity);
    }
}
