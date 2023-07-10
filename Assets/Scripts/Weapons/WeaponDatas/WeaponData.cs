using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponData : SkillData
{
    [field: SerializeField] public FireMode fireMode { get; private set; }
    [field: SerializeField] public ShootType shootType { get; private set; }
    [field: SerializeField] public HitEffect[] hitEffects { get; private set; }
    [field: SerializeField] public Weapon weaponPrefab { get; private set; }
    [field: SerializeField] public BaseWeaponStatsData stats { get; private set; }
    public override int MaxLevel => BonusPerLevel.Length;
    [field: SerializeField] public WeaponBonusPerLevel[] BonusPerLevel { get; private set; }
    public string GetBonusDescription(int level)
    {
        WeaponBonusPerLevel levelupBonus = BonusPerLevel[level];
        StringBuilder sb = new StringBuilder();
        foreach (var bonus in levelupBonus.bonuses)
        {
            bonus.BonusDescrition(sb);
        }
        return sb.ToString();
    }

}

