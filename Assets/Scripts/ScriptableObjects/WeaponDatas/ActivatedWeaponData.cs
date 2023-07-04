using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedWeaponData : WeaponData
{
    [field: SerializeField] public FlameThrower weaponPrefab { get; private set; }

    [field: SerializeField] private BaseWeaponStatsData stats { get; set; }
    public override BaseWeaponStatsData Stats => stats;
    public override Weapon WeaponPrefab => weaponPrefab;
}
