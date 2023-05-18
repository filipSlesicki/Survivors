using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Weapon
{
    [SerializeField] GameObject[] flames;
    [SerializeField] float damageCooldown = 0.1f;
    List<Entity> targetsInRange = new List<Entity>();

    float duration;
    float nextDamageTime;
    float deactivateTime;

    public override void Setup(WeaponData data,Character owner)
    {
        base.Setup(data,owner);
        duration = data.BaseStats.Duration;
    }
    public override void Shoot()
    {
        base.Shoot();
        ActivateFlames();
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
            target.TakeDamage(new DamageData(damage, this));
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
        WeaponStats bonus = data.BonusPerLevel[level -1];
        if (bonus.Size > 0)
        {
            foreach (var flame in flames)
            {
                flame.transform.localScale = Vector2.one * sizeModifier;
            }
        }
        if (bonus.Duration > 0)
        {
            duration += duration;
        }


    }
}
