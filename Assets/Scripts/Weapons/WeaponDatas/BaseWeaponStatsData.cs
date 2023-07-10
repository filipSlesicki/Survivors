using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponStats")]
public class BaseWeaponStatsData : ScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; } = 1;
    [field: SerializeField] public float CoolDown { get; private set; } = 1;
    [field: SerializeField] public int BulletCount { get; private set; } = 1;
    [field: SerializeField] public float Duration { get; private set; } = 5;

    [field: SerializeField] public float Range { get; private set; } = 5;
    [field: SerializeField] public float Size { get; private set; } = 1;
    [field: SerializeField] public int Penetration { get; private set; } = 1;
    [field: SerializeField] public float BulletSpeed { get; private set; } = 5;

    public void SetValues(float damage, float size, int bulletCount, float duration, float range, float cooldown, int penetration)
    {
        Damage = damage;
        CoolDown = cooldown;
        BulletCount = bulletCount;
        Duration = duration;
        Range = range;
        Size = size;
        Penetration = penetration;
    }
}
