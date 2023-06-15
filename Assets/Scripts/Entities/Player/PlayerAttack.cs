using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public void Tick()
    {
        Enemy target = EnemyManager.GetClosestEnemyFromPoint(Player.Instance.movement.lastPosition);

        foreach (var weapon in PlayerSkills.Instance.weapons.Values)
        {
            if (weapon.aimAtClosest)
            {
                if (target)
                {
                    weapon.AimWeapon(target.movement.lastPosition);
                }

                weapon.Tick();
            }
        }
    }
}
