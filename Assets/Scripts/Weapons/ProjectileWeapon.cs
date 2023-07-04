using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileWeapon : Weapon
{
    Projectile projectilePrefab;
    protected float projectileSpeed { get { return stats[WeaponStatType.BulletSpeed]; } set { stats[WeaponStatType.BulletSpeed] = value; } }
    ObjectPool<Projectile> bulletPool;

    int bulletLayer;

    protected override void SetStats(WeaponData data)
    {
        base.SetStats(data);
        ProjectileWeaponData projectileData = data as ProjectileWeaponData;
        if (projectileData != null)
        {
            ProjectileWeaponStats projectileStats = data.Stats as ProjectileWeaponStats;
            projectilePrefab = projectileStats.BulletPrefab;
            stats[WeaponStatType.BulletSpeed] = projectileStats.BulletSpeed;
        }
        else
        {
            Debug.LogError("Wrong weapon data");
        }
        bulletPool = new ObjectPool<Projectile>(CreateProjectile, OnGetPrjectile, OnReleaseProjectile, null, false, 30);

        for (int i = 0; i < 10; i++)
        {
            Projectile newProjectile = CreateProjectile();
            bulletPool.Release(newProjectile);
        }
    }

    protected override void SetOwner(Character owner)
    {
        base.SetOwner(owner);
        if (owner is Player)
        {
            bulletLayer = LayerMask.NameToLayer("PlayerBullet");
        }
        else if (owner is Enemy)
        {
            bulletLayer = LayerMask.NameToLayer("EnemyBullet");
        }
    }

    public override void Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Projectile projectile = bulletPool.Get();
            projectile.transform.SetPositionAndRotation(shootPoints[i].position, shootPoints[i].rotation);
            projectile.Launch(projectileSpeed, damage, this,penetration,duration);
        }
      
        base.Shoot();
    }

    Projectile CreateProjectile()
    {
        Projectile newProjectile = Instantiate(projectilePrefab);
        newProjectile.bulletPool = bulletPool;

        return newProjectile;
    }

    void OnGetPrjectile(Projectile projectile)
    {
        projectile.transform.localScale = Vector2.one * size;
        projectile.gameObject.SetActive(true);
        projectile.gameObject.layer = bulletLayer;
    }

    void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

}
