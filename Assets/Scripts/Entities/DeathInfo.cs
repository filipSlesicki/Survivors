using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DeathInfo 
{
    public Entity killer;
    public Entity killed;

    public DeathInfo(Entity killed, Entity killer)
    {
        this.killed = killed;
        this.killer = killer;
    }
}
