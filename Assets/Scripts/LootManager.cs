using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField]GameObject expPickup;
    private void OnEnable()
    {
        Enemy.OnEnemyDead += SpawnLoot;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDead -= SpawnLoot;
    }

    void SpawnLoot(Enemy e)
    {
        Instantiate(expPickup, e.transform.position, Quaternion.identity);
    }
}
