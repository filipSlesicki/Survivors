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

    public static event Action<GameObject> OnPlayerDead;

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


    public override void Die(GameObject attacker)
    {
        OnPlayerDead?.Invoke(attacker);
        gameObject.SetActive(false);
    }
}
