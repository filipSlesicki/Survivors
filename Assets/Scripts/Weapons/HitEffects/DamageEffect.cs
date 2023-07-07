using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/HitEffects/Damage Effect")]
public class DamageEffect : HitEffect
{
    public override void OnHit(Collider2D hitCol, Weapon weapon)
    {
        Entity damagable;
        if (hitCol.TryGetComponent<Entity>(out damagable))
        {
            damagable.TakeDamage(new DamageInfo(weapon.damage, weapon));
        }
        
    }
}
