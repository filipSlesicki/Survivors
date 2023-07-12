using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class MovingProjectile : Projectile
{
    public override void Launch(Weapon weapon, Color color)
    {
        base.Launch(weapon, color);
        rb.velocity = transform.right * weapon.projectileSpeed;

    }    
}
