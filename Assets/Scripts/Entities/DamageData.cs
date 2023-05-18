using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData 
{
    public float damage;
    public GameObject attacker;
    public GameObject attacked;
    public Weapon weapon;

    public DamageData(float damage, GameObject attacker)
    {
        this.damage = damage;
        this.attacker = attacker;
        weapon = null;
        attacked = null;
    }

    public DamageData(float damage, Weapon weapon)
    {
        this.damage = damage;
        this.weapon = weapon;
        attacker = null;
        attacked = null;
    }
}
