using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class HitEffect : ScriptableObject
{
    public abstract void OnHit(Collider2D hitCol, Weapon weapon);
}
