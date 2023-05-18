using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public UnityEvent<float, float> OnHealthChanged;
    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(DamageData damageData)
    {
        currentHealth -= damageData.damage;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die(damageData.attacker);
        }
    }

    public virtual void Heal(float amount)
    {
        currentHealth += amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void Die(GameObject attacker)
    {
    }

}
