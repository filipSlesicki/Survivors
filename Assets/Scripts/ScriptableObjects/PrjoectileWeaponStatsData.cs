using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats/Proejctile Weapon Stats", fileName = "Projectile Weapon Stats", order = 1)]
public class PrjoectileWeaponStatsData : WeaponStatsData
{
    [field: SerializeField] public float BulletSpeed { get; private set; }

}
