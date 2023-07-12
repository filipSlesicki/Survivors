using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomEnemyChance : ScriptableObject
{
    public List<EnemyChance> enemies = new List<EnemyChance>();
    public Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };

    public Enemy GetRandomEnemy()
    {
        float random = Random.value;
        float totalChance = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            totalChance += enemies[i].dropChance;
            if( random <= totalChance)
            {
                return enemies[i].enemy;
            }
        }
        return enemies[0].enemy;
    }
}

[System.Serializable]
public class EnemyChance
{
    public Enemy enemy;
    public float dropChance;
}

