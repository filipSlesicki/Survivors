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

#if UNITY_EDITOR
    [MenuItem("Assets/Copy stats to scriptable")]
    public void CopyToGunStats()
    {
        //if(BaseStatsData == null)
        //{
        //    BaseWeaponStatsData newStatsData = ScriptableObject.CreateInstance<BaseWeaponStatsData>();
        //    string path = AssetDatabase.GetAssetPath(this);
        //    path = path.Replace(".asset", "Stats.asset");
        //    AssetDatabase.CreateAsset(newStatsData, path);

        //    BaseStatsData = newStatsData;
        //    //Create scriptable object
        //}
        //BaseStatsData.SetValues(Stats.Damage, Stats.Size, Stats.BulletCount,
        //    Stats.Duration, Stats.CoolDown, Stats.Penetrating);
        //AssetDatabase.SaveAssets();
    }
#endif

    public void ClearStatsData()
    {
        //BaseStatsData = null;
        //string path = AssetDatabase.GetAssetPath(this);
        //path = path.Replace(".asset", "Stats.asset");
        //AssetDatabase.DeleteAsset(path);
    }

}

