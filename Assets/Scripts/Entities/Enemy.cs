using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character
{

    [SerializeField] EnemyAttack attack;
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

        if (toPlayer.sqrMagnitude < attack.range * attack.range)
        {
           attack.Attack(Player.Instance);
        }

        var cellUnder = GridController.curFlowField.GetCellFromWorldPos(movement.lastPosition);
        movement.SetDirection(cellUnder.BestNormalizedDirection);


    }

    public void SimpleTick(float dt)
    {
        var cellUnder = GridController.curFlowField.GetCellFromWorldPos(movement.lastPosition);
        movement.SetDirection(cellUnder.BestNormalizedDirection);
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
