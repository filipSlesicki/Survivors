using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    [SerializeField] TrailRenderer trailPrefab;
    public LayerMask hitLayer;
    float rayWidth = 0.1f;

    public override void Setup(WeaponData data, Character owner)
    {
        base.Setup(data,owner);
        rayWidth = data.BaseStats.Size;
        trailPrefab.startWidth = rayWidth;
        trailPrefab.endWidth = rayWidth;
    }

    public override void Shoot()
    {
        base.Shoot();
        if(!penetrating)
        {
            ShootSingle();
        }
        else
        {
            ShootAll();
        }
    }

    void ShootSingle()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            RaycastHit2D hit = Physics2D.CircleCast(shootPoints[i].position,rayWidth * sizeModifier, shootPoints[i].right, range, hitLayer);
            if (hit.collider)
            {
                Entity hitHealth;
                if (hit.collider.TryGetComponent<Entity>(out hitHealth))
                {
                    hitHealth.TakeDamage(new DamageData(damage,this));
                }

                TrailRenderer trail = Instantiate(trailPrefab);
                trail.AddPosition(shootPoints[i].position);
                trail.transform.position = hit.point;
            }
            else
            {
                ShowMaxTrail(i);
            }
        }
      
    }

    void ShootAll()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(shootPoints[i].position,rayWidth * sizeModifier, shootPoints[i].right, range, hitLayer);
            foreach (var hit in hits)
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider)
                {
                    Entity hitHealth;
                    if (hit.collider.TryGetComponent<Entity>(out hitHealth))
                    {
                        hitHealth.TakeDamage(new DamageData(damage, this));
                    }
                }

            }
            ShowMaxTrail(i);
        }
      
    }

    void ShowMaxTrail(int shotIndex)
    {
        TrailRenderer trail = Instantiate(trailPrefab);
        trail.widthMultiplier = sizeModifier;
        trail.AddPosition(shootPoints[shotIndex].position);
        trail.transform.position = shootPoints[shotIndex].position + shootPoints[shotIndex].right * range; ;
    }
}
