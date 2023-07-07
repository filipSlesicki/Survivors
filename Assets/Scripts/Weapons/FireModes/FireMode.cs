using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FireMode : ScriptableObject
{
    public abstract void Shoot(Weapon weapon);
    public virtual void Update() { }
}
