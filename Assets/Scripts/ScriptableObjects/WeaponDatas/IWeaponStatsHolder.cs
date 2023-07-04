using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponStatsHolder<T> where T : BaseWeaponStatsData
{
    T GetStats { get; }
}
