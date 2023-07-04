using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Raycast Weapon")]
public class RaycastWeaponData : WeaponData
{
    [field: SerializeField] public RaycastWeapon weaponPrefab { get; private set; }
    [field: SerializeField] private RaycastWeaponStats stats{ get; set; }


    public override Weapon WeaponPrefab => weaponPrefab;
    public override BaseWeaponStatsData Stats => stats ;

}
