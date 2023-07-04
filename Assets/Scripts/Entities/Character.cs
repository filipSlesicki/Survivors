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
    protected void HandleBonusGain(CharacterStats stat, PassiveBonusInfo bonus)
    {
        switch (stat)
        {
            case CharacterStats.Health:
                Bonuses.ApplyBonusToValue(ref maxHealth, bonus);
                Bonuses.ApplyBonusToValue(ref currentHealth, bonus);
                //OnHealthChanged?.Invoke(currentHealth, maxHealth);
                break;
            case CharacterStats.Speed:
                movement.ApplySpeedBonus(bonus);
                break;
            default:
                break;
        }
    }

    protected void HandleBonusLost(CharacterStats stat, PassiveBonusInfo bonus)
    {

    }
}
