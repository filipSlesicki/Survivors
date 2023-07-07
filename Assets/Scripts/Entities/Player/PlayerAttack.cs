using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    public Vector3Event OnClosestEnemyFoundEvent;
    public static Vector3 closestEnemyPos;
    public void Tick()
    {
        Enemy target = EnemyManager.GetClosestEnemyFromPoint(Player.Instance.movement.lastPosition);
        if(target)
        { 
            closestEnemyPos = target.movement.lastPosition;
        }    
        foreach (var weapon in PlayerSkills.Instance.weapons.Values)
        {
            //if(target)
            //{
            //    if(weapon.aimAtClosest)
            //    { 
            //        weapon.AimWeapon(target.movement.lastPosition);
            //    }    
            //}
            weapon.Tick();
        }
    }
}
