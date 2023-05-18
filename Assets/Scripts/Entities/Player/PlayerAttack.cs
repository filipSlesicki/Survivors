using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
   public void Tick()
    {
        foreach (var weapon in PlayerSkills.Instance.weapons.Values)
        {
            weapon.Tick();
        }
    }
}
