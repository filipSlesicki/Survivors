using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Weapon Stats")]
public class PrjoectileWeaponStatsData : WeaponStatsData
{
    [field: SerializeField] public float BulletSpeed { get; private set; }

}
