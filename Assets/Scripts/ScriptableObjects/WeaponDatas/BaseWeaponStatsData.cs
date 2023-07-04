using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats/Base Weapon Stats", fileName = "Base Weapon Stats")]
public class BaseWeaponStatsData : ScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; } = 1;
    [field: SerializeField] public float CoolDown { get; private set; } = 1;
    [field: SerializeField] public int BulletCount { get; private set; } = 1;
    [field: SerializeField] public float Duration { get; private set; } = 5;
    [field: SerializeField] public float Size { get; private set; } = 1;
    [field: SerializeField] public int Penetration { get; private set; } = 1;

    public void SetValues(float damage, float size, int bulletCount, float duration, float cooldown, int penetration)
    {
        Damage = damage;
        CoolDown = cooldown;
        BulletCount = bulletCount;
        Duration = duration;
        Size = size;
        Penetration = penetration;
    }
}
