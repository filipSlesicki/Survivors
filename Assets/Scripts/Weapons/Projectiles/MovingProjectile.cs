using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class MovingProjectile : Projectile
{
    public void Launch(float speed, float damage, Weapon owner, int penetration, float lifeTime)
    {
        this.weapon = owner;
        rb.velocity = transform.right * speed;
        this.penetration = penetration;
        destroyTime = Time.time + lifeTime;
    }

    public override void Launch(Weapon weapon, ObjectPool<Projectile> bulletPool)
    {
        base.Launch(weapon, bulletPool);
        rb.velocity = transform.right * weapon.projectileSpeed;

    }    
}
