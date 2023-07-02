using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEditor;

[CreateAssetMenu(menuName = "Weapons/BaseWeapon")]
public class WeaponData : SkillData
{
    [field: SerializeField] public Weapon WeaponPrefab { get; private set; }
    [field: SerializeField] public WeaponStats BaseStats { get; private set; }
    [field: SerializeField] public WeaponStatsData BaseStatsData { get; private set; }
    [field: SerializeField] public WeaponStats[] BonusPerLevel { get; private set; }
    public override int MaxLevel => BonusPerLevel.Length;
    [HideInInspector]
    public int TestInt;


    public string GetBonusDescription(int level)
    {
        WeaponStats bonus = BonusPerLevel[level];
        StringBuilder sb = new StringBuilder();
        if (bonus.Damage > 0)
        {
            sb.AppendFormat("damage + {0} \n", bonus.Damage);
        }
        if (bonus.Size > 0)
        {
            sb.AppendFormat("size + {0} \n", bonus.Size);
        }
        if (bonus.BulletCount > 0)
        {
            sb.AppendFormat("bulletCount + {0} \n", bonus.BulletCount);
        }
        if (bonus.Duration > 0)
        {
            sb.AppendFormat("duration + {0} \n", bonus.Duration);
        }
        if (bonus.CoolDown > 0)
        {
            sb.AppendFormat("coolDown - {0}% \n", bonus.CoolDown);
        }
        if (bonus.Penetrating == true)
        {
            sb.Append("Add penetration \n");
        }

        return sb.ToString();
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Copy stats to scriptable")]
    public void CopyToGunStats()
    {
        if(BaseStatsData == null)
        {
            WeaponStatsData newStatsData = ScriptableObject.CreateInstance<WeaponStatsData>();
            string path = AssetDatabase.GetAssetPath(this);
            path = path.Replace(".asset", "Stats.asset");
            AssetDatabase.CreateAsset(newStatsData, path);

            BaseStatsData = newStatsData;
            //Create scriptable object
        }
        BaseStatsData.SetValues(BaseStats.Damage, BaseStats.Size, BaseStats.BulletCount,
            BaseStats.Duration, BaseStats.CoolDown, BaseStats.Penetrating);
        AssetDatabase.SaveAssets();
    }
#endif

    public void ClearStatsData()
    {
        BaseStatsData = null;
        string path = AssetDatabase.GetAssetPath(this);
        path = path.Replace(".asset", "Stats.asset");
        AssetDatabase.DeleteAsset(path);
    }

}
[System.Serializable]
public class WeaponStats
{
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public int BulletCount { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    [field: SerializeField] public float CoolDown { get; private set; }
    [field: SerializeField] public bool Penetrating { get; private set; }
}

