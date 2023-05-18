using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers")]
public class Layers : ScriptableObject
{
    [SerializeField] LayerMask playerLayer;
    public static int Player;
    [SerializeField] LayerMask enemyLayer;
    public static int Enemy;

    private void OnValidate()
    {
        
        Player = playerLayer.value;
        Enemy = enemyLayer.value;
    }
}
