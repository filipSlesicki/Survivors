using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkills : MonoBehaviour
{
    public Dictionary<WeaponData, Weapon> weapons = new Dictionary<WeaponData, Weapon>();
    public Dictionary<PassiveSkillData, int> passiveSkills = new Dictionary<PassiveSkillData, int>();

    [SerializeField] Transform weaponParent;
    [SerializeField] WeaponData startWeapon;

    public static PlayerSkills Instance;

    public static event Action<SkillData, int> OnSkillSelected;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AddSkill(startWeapon);
    }

    public void AddSkill(SkillData skillData)
    {
        if(skillData is WeaponData)
        {
            WeaponData weaponData = skillData as WeaponData;
            AddWeapon(weaponData);
        }
        else if (skillData is PassiveSkillData)
        {
            PassiveSkillData passiveData = skillData as PassiveSkillData;
            if(!passiveSkills.ContainsKey(passiveData))
            {
                passiveSkills.Add(passiveData,0);
            }
            LevelUpPassiveBonus(passiveData);
        }
    }

    void AddWeapon(WeaponData weaponData)
    {
        if (weapons.ContainsKey(weaponData))
        {
            weapons[weaponData].AddLevel();
        }
        else
        {
            Weapon newWeapon = Instantiate(weaponData.WeaponPrefab, weaponParent);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.Setup(weaponData, Player.Instance);
            weapons.Add(weaponData, newWeapon);
        }
        OnSkillSelected?.Invoke(weaponData, weapons[weaponData].level);
    }

    void LevelUpPassiveBonus(PassiveSkillData data)
    {
        PassiveBonusInfo bonusInfo = new PassiveBonusInfo(data.IncreasePerLevel, data.IncreaseType);
        Player.Instance.bonuses.AddBonus(data.Stat, bonusInfo);
        passiveSkills[data]++;
        OnSkillSelected?.Invoke(data, passiveSkills[data]);
    }

    public int GetSkillLevel(SkillData data)
    {
        if(data is WeaponData)
        {
            WeaponData weaponData = data as WeaponData;
            Weapon weapon;
            if(weapons.TryGetValue( weaponData,out weapon))
            {
                return weapon.displayLevel;
            }
            else
            {
                return 0;
            }
        }
        else if(data is PassiveSkillData)
        {
            PassiveSkillData passiveData = data as PassiveSkillData;
            if (passiveSkills.TryGetValue(passiveData, out int lv))
            {
                return lv + 1;
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }

    public string GetSkillDescriptionText(SkillData data)
    {
        if (data is WeaponData)
        {
            WeaponData weaponData = data as WeaponData;
            Weapon weapon;
            if (weapons.TryGetValue(weaponData, out weapon))
            {
                return weaponData.GetBonusDescription(weapon.level);
            }
            else
            {
                return weaponData.Description;
            }
        }
        else
        {
            PassiveSkillData passiveData = data as PassiveSkillData;
            return passiveData.GetDescription();
        }
    }

}
