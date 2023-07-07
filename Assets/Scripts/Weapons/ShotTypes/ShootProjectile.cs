using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(menuName = "Weapons/Shoot Types/Shoot Projectile")]
public class ShootProjectile : ShootType
{
    public Projectile projectilePrefab;
    protected ObjectPool<Projectile> bulletPool;

    private void OnEnable()
    {
        if (bulletPool == null)
        {
            bulletPool = new ObjectPool<Projectile>(CreateProjectile, OnGetPrjectile, OnReleaseProjectile, null, false, 30);
        }
    }
    public override void Shoot(Weapon weapon)
    {
        var shootPoints = weapon.shootPositions;
        for (int i = 0; i < weapon.bulletCount; i++)
        {
            Projectile projectile = bulletPool.Get();
            projectile.transform.SetPositionAndRotation(shootPoints[i].position, shootPoints[i].rotation);
            projectile.Launch(weapon,bulletPool);
        }
    }

    Projectile CreateProjectile()
    {
        Projectile newProjectile = Instantiate(projectilePrefab);
        return newProjectile;
    }

    protected virtual void OnGetPrjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    protected virtual void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
}
