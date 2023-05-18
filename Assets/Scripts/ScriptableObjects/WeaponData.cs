using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class WeaponData : SkillData
{
    [field: SerializeField] public Weapon WeaponPrefab { get; private set; }
    [field: SerializeField] public WeaponStats BaseStats { get; private set; }
    [field: SerializeField] public WeaponStats[] BonusPerLevel { get; private set; }
    public override int MaxLevel => BonusPerLevel.Length;


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

