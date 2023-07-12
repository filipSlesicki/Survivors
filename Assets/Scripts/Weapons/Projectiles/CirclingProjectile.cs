using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CirclingProjectile : Projectile
{
    float angle;

    public override void Launch(Weapon weapon, Color color)
    {
        base.Launch(weapon, color);
        Vector3 toOwner = transform.position - weapon.transform.position;
        angle = Vector3.SignedAngle(toOwner, Vector3.right, Vector3.forward);
    }
    private void FixedUpdate()
    {
        angle +=  weapon.projectileSpeed * Time.deltaTime;

        Vector3 position = weapon.transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * weapon.range,
            Mathf.Sin(Mathf.Deg2Rad * angle) * weapon.range,
            0);

       rb.MovePosition( position);
        
    }
}
