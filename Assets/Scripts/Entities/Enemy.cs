using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character
{
    [SerializeField] float damage = 1;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float attackRange = 0.5f;
    private float nextAttackTime;

    public void Setup(int level)
    {
        maxHealth *= 1 + level * 0.1f;
        currentHealth = maxHealth;
        movement.ApplySpeedBonus(new PassiveBonusInfo(level * 0.05f, IncreaseType.Additive));
        transform.localScale = Vector3.one * (1 + level * 0.1f);
    }
    Vector2 toPlayer;
    public void Tick(float dt)
    {
        toPlayer = Player.Instance.movement.lastPosition - movement.lastPosition;

        if (toPlayer.sqrMagnitude < attackRange * attackRange)
        {
            Attack(Player.Instance);
        }

        toPlayer.Normalize();
        movement.SetDirection(toPlayer);


    }

    public void SimpleTick(float dt)
    { 
        toPlayer = Player.Instance.movement.lastPosition - movement.lastPosition;
        toPlayer.Normalize();
        movement.SetDirection(toPlayer);
    }

    void Attack(Entity target)
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }
        target.TakeDamage(new DamageData(damage, gameObject));
        nextAttackTime = Time.time + attackCooldown;
    }

    #region Health
    public override void TakeDamage(DamageData damageData)
    {
        damageData.attacked = gameObject;

        OnEnemyDamaged?.Invoke(damageData);
        base.TakeDamage(damageData);
    }

    public override void Die(GameObject killer)
    {
        OnEnemyDead?.Invoke(this);
        Destroy(gameObject);
    }
    #endregion

    public static event Action<DamageData> OnEnemyDamaged;
    public static event Action<Enemy> OnEnemyDead;
}
