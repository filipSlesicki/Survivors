using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/Fire Modes/Activate")]
public class OnOffWeapon : FireMode
{
    public float tickRate = 0.1f;
    WaitForSeconds tickWait;
    public LineData line;

    private void OnEnable()
    {
        tickWait = new WaitForSeconds(tickRate);
    }
    public override void Shoot(Weapon weapon)
    {
        weapon.StartCoroutine(ActivateWeapon(weapon));
    }

    IEnumerator ActivateWeapon(Weapon weapon)
    {
        if (line)
            line.MakeLines(weapon);

        int ticks = Mathf.FloorToInt( weapon.duration / tickRate);
        for (int i = 0; i < ticks; i++)
        {
            weapon.DoWeaponEffect();
            yield return tickWait;
        }
        weapon.onStopShooting?.Invoke();

        if (line)
            line.DestroyLines(weapon);
    }
}
