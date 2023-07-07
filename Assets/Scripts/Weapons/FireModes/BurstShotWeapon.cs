using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Fire Modes/Burst")]
public class BurstShotWeapon : FireMode
{
    public float timeBetweenBursts = 0.1f;
    public int burstCount;
    WaitForSeconds waitForBurst;
    private void OnEnable()
    {
        waitForBurst = new WaitForSeconds(timeBetweenBursts);
    }
    public override void Shoot(Weapon weapon)
    {
        weapon.StartCoroutine(ShootBurst(weapon));
    }

    IEnumerator ShootBurst(Weapon weapon)
    {
        for (int i = 0; i < burstCount; i++)
        {
            weapon.DoWeaponEffect();
            yield return waitForBurst;
        }
        weapon.onStopShooting?.Invoke();

    }
}
