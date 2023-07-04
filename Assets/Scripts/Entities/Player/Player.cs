using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class Player : Character
{
    PlayerInput input;
    InputAction moveAtion;
    public static Player Instance;
    [SerializeField] PlayerAttack attack;

    public FloatEvent onHealthChangedEvent;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        input = GetComponent<PlayerInput>();
        moveAtion = input.actions["Move"];
    }

    private void OnEnable()
    {
        bonuses.OnBonusGained += HandleBonusGain;
        bonuses.OnBonusGained += HandleBonusLost;
    }

    private void OnDisable()
    {
        bonuses.OnBonusGained -= HandleBonusGain;
        bonuses.OnBonusGained -= HandleBonusLost;
    }

    void Update()
    {

        attack.Tick();
        bonuses.UpdateTemporaryBonuses(Time.time);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 moveVector = moveAtion.ReadValue<Vector2>();
        if (moveVector.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (moveVector.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        movement.SetDirection(moveVector);
    }
    public override void TakeDamage(DamageInfo damageInfo)
    {
        base.TakeDamage(damageInfo);
        onHealthChangedEvent.Raise(currentHealth / maxHealth);
    }
    public  void Heal(float amount)
    {
        currentHealth += amount;
        onHealthChangedEvent.Raise(currentHealth / maxHealth);
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public override void Die(Entity attacker)
    {
        base.Die(attacker);
        gameObject.SetActive(false);
    }
}
