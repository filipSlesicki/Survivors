using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5;
    [HideInInspector] public Vector2 lastPosition;

    public abstract void Move(Vector2 direction, float dt);
    public abstract void SetDirection(Vector2 direction);
    public void ApplySpeedBonus(PassiveBonusInfo bonus)
    {
        Bonuses.ApplyBonusToValue(ref moveSpeed, bonus);
    }
}
