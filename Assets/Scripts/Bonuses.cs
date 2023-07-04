using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bonuses
{
    Dictionary<CharacterStats,List<PassiveBonusInfo>> passiveBonuses = new Dictionary<CharacterStats, List<PassiveBonusInfo>>();
    public event Action<CharacterStats, PassiveBonusInfo> OnBonusGained;
    public event Action<CharacterStats, PassiveBonusInfo> OnBonusLost;

    //public Bonuses()
    //{
    //    foreach (PlayerStats stat in Enum.GetValues(typeof( PlayerStats)))
    //    {
    //        passiveBonuses.Add(stat, new List<PassiveBonusInfo>());
    //    }
    //}


    public void AddBonus(CharacterStats stat, PassiveBonusInfo bonusInfo)
    {
        if(!passiveBonuses.ContainsKey(stat))
        {
            passiveBonuses.Add(stat, new List<PassiveBonusInfo>());
        }

        passiveBonuses[stat].Add(bonusInfo);

        OnBonusGained?.Invoke(stat, bonusInfo);
    }

    public void UpdateTemporaryBonuses(float currentTime)
    {
        foreach (var passiveBonus in passiveBonuses)
        {
            var bonusList = passiveBonus.Value;
            for (int i = bonusList.Count-1; i >= 0; i--)
            {
                if(bonusList[i].timed)
                {
                   if(bonusList[i].removeTime <= currentTime)
                    {
                        RemoveBonus(passiveBonus.Key, bonusList[i]);
                    }
                }
            }
        }
    }

    public void ApplyAllBonuses(CharacterStats stat,ref float baseValue)
    {
        if (!passiveBonuses.ContainsKey(stat))
            return;

        foreach (var bonus in passiveBonuses[stat])
        {
            ApplyBonusToValue(ref baseValue, bonus.increaseType,bonus.value);
        }
    }

    public static void ApplyBonusToValue(ref float baseValue, IncreaseType increaseType, float increaseValue)
    {
        switch (increaseType)
        {
            case IncreaseType.Additive:
                baseValue += increaseValue;
                break;
            case IncreaseType.Multiplicative:
                baseValue *= increaseValue;
                break;
            case IncreaseType.AddPercent:
                baseValue *= 1 + increaseValue / 100;
                break;
            case IncreaseType.Set:
                baseValue = increaseValue;
                break;
            default:
                break;
        }
    }

    public static void ApplyBonusToValue(ref float baseValue, PassiveBonusInfo bonus)
    {
        ApplyBonusToValue(ref baseValue, bonus.increaseType, bonus.value);
    }

    void RemoveBonus(CharacterStats stat, PassiveBonusInfo bonusInfo)
    {
        if(!passiveBonuses.ContainsKey(stat))
        {
            Debug.LogWarning("Trying to remove bonus that doesnt exist");
            return;
        }
        passiveBonuses[stat].Remove(bonusInfo);
        OnBonusLost?.Invoke(stat, bonusInfo);
    }
}

public struct PassiveBonusInfo
{
    public float value;
    public IncreaseType increaseType;
    public bool timed;
    public float removeTime;

    public PassiveBonusInfo(float value,IncreaseType increaseType, bool timed = false, float removeTime =0 )
    {
        this.value = value;
        this.increaseType = increaseType;
        this.timed = timed;
        this.removeTime = removeTime;

    }
}


