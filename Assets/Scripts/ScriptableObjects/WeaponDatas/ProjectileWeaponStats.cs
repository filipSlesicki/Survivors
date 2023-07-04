using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats/Projectile Weapon Stats")]
public class ProjectileWeaponStats : BaseWeaponStatsData
{
    [field: SerializeField] public Projectile BulletPrefab { get; private set; }
    [field: SerializeField] public float BulletSpeed { get; private set; }

}
