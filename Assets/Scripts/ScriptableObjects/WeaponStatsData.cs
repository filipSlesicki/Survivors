using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats/Base Weapon Stats", fileName = "Base Weapon Stats")]
public class WeaponStatsData : ScriptableObject
{
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public int BulletCount { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public float CoolDown { get; private set; }
    [field: SerializeField] public bool Penetrating { get; private set; }

    public void SetValues(float damage, float size, int bulletCount, float duration, float cooldown, bool penetrating)
    {
        Damage = damage;
        Size = size;
        BulletCount = bulletCount;
        Duration = duration;
        CoolDown = cooldown;
        Penetrating = penetrating;
    }
}
