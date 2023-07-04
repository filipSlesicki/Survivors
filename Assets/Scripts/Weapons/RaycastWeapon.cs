using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon
{
    [SerializeField] TrailRenderer trailPrefab;
    int hitLayer;
    float range;
    protected override void SetStats(WeaponData data)
    {
        base.SetStats(data);
        SetTrailWidth();
        RaycastWeaponData raycastWeaponData = data as RaycastWeaponData;
        if(raycastWeaponData)
        {
            RaycastWeaponStats raycastStats = data.Stats as RaycastWeaponStats;
            range = raycastStats.Range;
        }
    }

    protected override void SetOwner(Character owner)
    {
        base.SetOwner(owner);
        if (owner is Player)
        {
            hitLayer = LayerMask.GetMask("Enemy");
        }
        else if (owner is Enemy)
        {
            hitLayer = LayerMask.GetMask("Player");
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        if(penetration == 0)
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
            RaycastHit2D hit = Physics2D.CircleCast(shootPoints[i].position, size, shootPoints[i].right, range, hitLayer);
            if (hit.collider)
            {
                Entity hitHealth;
                if (hit.collider.TryGetComponent<Entity>(out hitHealth))
                {
                    hitHealth.TakeDamage(new DamageInfo(damage, this));
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
            RaycastHit2D[] hits = Physics2D.CircleCastAll(shootPoints[i].position, size, shootPoints[i].right, range, hitLayer);
            foreach (var hit in hits)
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider)
                {
                    Entity hitHealth;
                    if (hit.collider.TryGetComponent<Entity>(out hitHealth))
                    {
                        hitHealth.TakeDamage(new DamageInfo(damage, this));
                    }
                }

            }
            ShowMaxTrail(i);
        }
      
    }
    public override void AddLevel()
    {
        base.AddLevel();
    }

    void SetTrailWidth()
    {
        trailPrefab.startWidth = size;
        trailPrefab.endWidth = size;
    }

    void ShowMaxTrail(int shotIndex)
    {
        TrailRenderer trail = Instantiate(trailPrefab);
        trail.widthMultiplier = size;
        trail.AddPosition(shootPoints[shotIndex].position);
        trail.transform.position = shootPoints[shotIndex].position + shootPoints[shotIndex].right * range; ;
    }
}
