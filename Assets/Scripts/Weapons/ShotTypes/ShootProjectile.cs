using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shoot Types/Shoot Projectile")]
public class ShootProjectile : ShootType
{
    public Projectile projectilePrefab;
    public Color color;

    public override void Shoot(Weapon weapon)
    {
        var shootPoints = weapon.shootPositions;
        for (int i = 0; i < weapon.bulletCount; i++)
        {
            Projectile projectile = PoolManager.Get(projectilePrefab);
            projectile.transform.SetPositionAndRotation(shootPoints[i].position, shootPoints[i].rotation);
            projectile.Launch(weapon, color);
        }
    }
}
