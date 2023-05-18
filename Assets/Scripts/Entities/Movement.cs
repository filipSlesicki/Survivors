using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5;

    public abstract void Move(Vector3 direction);

    public void ApplySpeedBonus(PassiveBonusInfo bonus)
    {
        Bonuses.ApplyBonusToValue(ref moveSpeed, bonus);
    }
}
