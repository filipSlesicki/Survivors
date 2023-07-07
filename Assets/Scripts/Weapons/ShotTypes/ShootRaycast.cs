using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shoot Types/Boxcast Single")]
public class ShootRaycast : ShootType
{
    [SerializeField] LineData line;

    public override void Shoot(Weapon weapon)
    {
        int targetLayer = weapon.GetTargetLayer();
        Vector2 raySize = new Vector2(weapon.size, weapon.size);
        var shootPoints = weapon.shootPositions;
        for (int i = 0; i < weapon.bulletCount; i++)
        {
            RaycastHit2D hit = Physics2D.BoxCast(shootPoints[i].position, raySize, 0,
                shootPoints[i].right, weapon.range, targetLayer);

           
            if(hit.collider != null)
            {
                if(line)
                {
                    line.UpdateLine(weapon, i, new Vector3(hit.distance, 0, 0));
                }
                //weapon.onUpdateHitPoint?.Invoke(i, new Vector3(hit.distance, 0, 0));
                foreach (var effect in weapon.data.hitEffects)
                {
                    effect.OnHit(hit.collider, weapon);
                }
            }
            else
            {
                line.UpdateLine(weapon, i, new Vector3(weapon.range, 0, 0));
                //weapon.onUpdateHitPoint?.Invoke(i, new Vector3(weapon.range,0,0));
            }

        }
    }

}
