using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public DamageInfoEvent onDamagedEvent;
    public DeathInfoEvent onDeathEvent;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(DamageInfo damageInfo)
    {
        currentHealth -= damageInfo.damage;
        damageInfo.attacked = this;
        onDamagedEvent.Raise(damageInfo);
        if (currentHealth <= 0)
        {
            Die(damageInfo.attacker);
        }
    }



    public virtual void Die(Entity killer)
    {
        onDeathEvent.Raise(new DeathInfo(this, killer));
    }

}
