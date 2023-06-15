using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats")]
public class WeaponStatsData : ScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public int BulletCount { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public float CoolDown { get; private set; }
    [field: SerializeField] public bool Penetrating { get; private set; }
}
