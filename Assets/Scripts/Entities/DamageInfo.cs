using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageInfo 
{
    public float damage;
    public Entity attacker;
    public Entity attacked;
    public Weapon weapon;

    public DamageInfo(float damage, Entity attacker)
    {
        this.damage = damage;
        this.attacker = attacker;
        weapon = null;
        attacked = null;
    }

    public DamageInfo(float damage, Weapon weapon)
    {
        this.damage = damage;
        this.weapon = weapon;
        attacker = null;
        attacked = null;
    }
}
