using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootType : ScriptableObject
{
    public abstract void Shoot(Weapon weapon);
}
