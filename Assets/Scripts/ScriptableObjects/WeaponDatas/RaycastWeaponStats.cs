using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats/Raycast Weapon Stats")]
public class RaycastWeaponStats : BaseWeaponStatsData
{
    [field: SerializeField] public float Range { get; private set; }
}
