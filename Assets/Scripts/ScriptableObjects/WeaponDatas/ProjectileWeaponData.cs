using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Projectile Weapon")]

public class ProjectileWeaponData : WeaponData
{

    [field: SerializeField] private ProjectileWeaponStats stats { get; set; }
    [field: SerializeField] public ProjectileWeapon weaponPrefab { get; private set; }

    public override Weapon WeaponPrefab => weaponPrefab;
    public override BaseWeaponStatsData Stats => stats;
}
