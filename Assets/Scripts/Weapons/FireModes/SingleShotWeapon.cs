using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Fire Modes/Single")]
public class SingleShotWeapon : FireMode
{
    public LineData line;
    public override void Shoot(Weapon weapon)
    {
        if(line)
        {
            line.MakeLines(weapon);
        }
        weapon.DoWeaponEffect();
        weapon.StartCoroutine(StopAfterShortDelay(weapon));
    }
    IEnumerator StopAfterShortDelay(Weapon weapon)
    {
        yield return new WaitForSeconds(0.3f);
        weapon.onStopShooting?.Invoke();
        if(line)
        {
            line.DestroyLines(weapon);
        }
    }
}
