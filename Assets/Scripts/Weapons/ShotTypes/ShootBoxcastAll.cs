using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shoot Types/Boxcast All")]
public class ShootBoxcastAll : ShootType
{
    public override void Shoot(Weapon weapon)
    {
        int targetLayer = weapon.GetTargetLayer();
        Vector2 raySize = new Vector2(weapon.size, weapon.size);
        var shootPoints = weapon.shootPositions;
        for (int i = 0; i < weapon.bulletCount; i++)
        {
            int hitCount = Physics2D.BoxCastNonAlloc(shootPoints[i].position, raySize, 0, shootPoints[i].right,
                SharedHitBuffer.hitBuffer, weapon.range, targetLayer);

            for (int j = 0; j < hitCount; j++)
            {
                foreach (var effect in weapon.data.hitEffects)
                {
                    effect.OnHit(SharedHitBuffer.hitBuffer[j].collider, weapon);
                }
            }
        }
    }
}
