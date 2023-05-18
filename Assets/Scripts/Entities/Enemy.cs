using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character
{
    [SerializeField] float damage = 1;
    [SerializeField] float attackCooldown = 0.5f;

    private float nextAttackTime;

    public void Setup(int level)
    {
        maxHealth *= 1 + level * 0.1f;
        currentHealth = maxHealth;
        movement.ApplySpeedBonus(new PassiveBonusInfo(level * 0.05f, IncreaseType.Additive));
        transform.localScale = Vector3.one * (1 + level * 0.1f);
    }
    private void OnEnable()
    {
        EnemyManager.allEnemies.Add(this);
    }
    private void OnDisable()
    {
        EnemyManager.allEnemies.Remove(this);
    }

    public void Tick()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        Vector3 toPlayer = FindObjectOfType<Player>().transform.position - transform.position;
        toPlayer.Normalize();
        movement.Move(toPlayer);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Entity target;
        if( other.TryGetComponent<Entity>(out target))
        {
            Attack(target);
        }
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
