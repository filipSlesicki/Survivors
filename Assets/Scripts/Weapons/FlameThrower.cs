using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon
{
    [SerializeField] GameObject[] flames;
    [SerializeField] float damageCooldown = 0.1f;
    List<Entity> targetsInRange = new List<Entity>();
    protected float range { get { return stats[WeaponStatType.Range]; } set { stats[WeaponStatType.Range] = value; } }
    float nextDamageTime;
    float deactivateTime;

    public override void Shoot()
    {
        base.Shoot();
        ActivateFlames();
    }
    protected override void SetStats(WeaponData data)
    {
        base.SetStats(data);
        stats[WeaponStatType.Range] = 1;
    }

    protected override void AdditionalUpdate()
    {
        base.AdditionalUpdate();
        if(nextDamageTime < Time.time)
        {
            DealDamage();
            nextDamageTime = Time.time + damageCooldown;
        }
        if(deactivateTime < Time.time)
        {
            DeactivateFlames();
        }
    }

    void DealDamage()
    {
        foreach (var target in targetsInRange)
        {
            target.TakeDamage(new DamageInfo(damage, this));
        }
    }

    void ActivateFlames()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            flames[i].SetActive(true);
        }
        deactivateTime = Time.time + duration;
        nextDamageTime = 0;
    }

    void DeactivateFlames()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            flames[i].SetActive(false);
        }
        targetsInRange.Clear();
    }

    public void AddTarget(Entity target)
    {
        targetsInRange.Add(target);
    }

    public void RemoveTarget(Entity target)
    {
        targetsInRange.Remove(target);
    }

    public override void AddLevel()
    {
        base.AddLevel();
        if (size > 0)
        {
          
            foreach (var flame in flames)
            {
                Vector2 totalSize = new Vector2(range, size);
                flame.transform.localScale = totalSize;
            }
        }



    }
}
