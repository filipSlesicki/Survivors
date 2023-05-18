using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{
    public Bonuses bonuses;

    public Movement movement;

    protected override void Awake()
    {
        base.Awake();
        bonuses = new Bonuses();
    }
    protected void HandleBonusGain(PlayerStats stat, PassiveBonusInfo bonus)
    {
        switch (stat)
        {
            case PlayerStats.Health:
                Bonuses.ApplyBonusToValue(ref maxHealth, bonus);
                Bonuses.ApplyBonusToValue(ref currentHealth, bonus);
                OnHealthChanged?.Invoke(currentHealth, maxHealth);
                break;
            case PlayerStats.Speed:
                movement.ApplySpeedBonus(bonus);
                break;
            default:
                break;
        }
    }

    protected void HandleBonusLost(PlayerStats stat, PassiveBonusInfo bonus)
    {

    }
}
