using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponEndGameStatsUI : MonoBehaviour
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDamage;
    public TextMeshProUGUI weaponDPS;

    public void Set(Weapon weapon)
    {
        weaponName.text = string.Format("{0} lv{1}",weapon.data.name,weapon.displayLevel);
        weaponDamage.text = weapon.damageDealt.ToString();
        weaponDPS.text = (weapon.damageDealt / (GameTime.totalTimeInSeconds - weapon.aquireTime) ).ToString("0.00");

    }
}
