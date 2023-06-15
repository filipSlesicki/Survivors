using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] public float range = 0.5f;

    public virtual void Attack(Entity target)
    {

    }
}
