using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileWeapon : Weapon
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float projectileSpeed = 10;
    float lifeTime;
    ObjectPool<Projectile> bulletPool;
    int bulletLayer;

    private void Start()
    {
        lifeTime = range / projectileSpeed;
        bulletPool = new ObjectPool<Projectile>(CreateProjectile,OnGetPrjectile,OnReleaseProjectile,null,false,30);

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
        if(data.BaseStatsData != null)
        {
            PrjoectileWeaponStatsData projectileWeaponData = (PrjoectileWeaponStatsData)data.BaseStatsData;
            if(projectileWeaponData!= null)
            projectileSpeed = projectileWeaponData.BulletSpeed;
        }

    }

    public override void Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Projectile projectile = bulletPool.Get();
            projectile.transform.SetPositionAndRotation(shootPoints[i].position, shootPoints[i].rotation);
            projectile.Launch(projectileSpeed, damage, this, penetrating, lifeTime);
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
        projectile.transform.localScale = Vector2.one * sizeModifier;
        projectile.gameObject.SetActive(true);
        projectile.gameObject.layer = bulletLayer;
    }

    void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }


    public override void AddLevel()
    {
        WeaponStats bonus = data.BonusPerLevel[level];
        if (bonus.Duration > 0)
        {
            lifeTime += bonus.Duration;
        }
        base.AddLevel();

    }

}
