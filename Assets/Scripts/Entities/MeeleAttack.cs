using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttack : EnemyAttack
{
    [SerializeField] float damage = 1;
    [SerializeField] float attackCooldown = 0.5f;
    private float nextAttackTime;

    public override void Attack(Entity target)
    {
        base.Attack(target);

        if (Time.time < nextAttackTime)
        {
            return;
        }
        nextAttackTime = Time.time + attackCooldown;

        target.TakeDamage(new DamageData(damage, gameObject));
    }
}
