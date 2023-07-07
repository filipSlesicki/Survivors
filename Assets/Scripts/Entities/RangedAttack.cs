using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : EnemyAttack
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] Transform weaponParent;
    private Weapon weapon;
    private void Start()
    {
        Weapon newWeapon = Instantiate(weaponData.weaponPrefab, weaponParent);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.Setup(weaponData, GetComponent<Enemy>());
        weapon = newWeapon;
    }

    public override void Attack(Entity target)
    {
        base.Attack(target);
        if(weapon.aimAtClosest)
        {
            weapon.AimWeapon(target.transform.position);
        }

        weapon.Tick();
    }
}
