using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] float projectileSpeed = 10;
    float lifeTime;

    private void Start()
    {
        lifeTime = range / projectileSpeed;
    }

    public override void Shoot()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Projectile newProjectile = Instantiate(projectilePrefab, shootPoints[i].position, shootPoints[i].rotation);
            newProjectile.transform.localScale = Vector2.one * sizeModifier;
            newProjectile.Launch(projectileSpeed, damage, this, penetrating, lifeTime);
        }
      
        base.Shoot();
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
