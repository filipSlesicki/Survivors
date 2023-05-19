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
