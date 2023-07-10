using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Handles bonuses on a character. Created in the character class
/// </summary>
public class Bonuses
{
    Dictionary<CharacterStats,List<PassiveBonusInfo>> passiveBonuses = new Dictionary<CharacterStats, List<PassiveBonusInfo>>();
    public event Action<CharacterStats, PassiveBonusInfo> OnBonusGained;
    public event Action<CharacterStats, PassiveBonusInfo> OnBonusLost;
    

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
    void RemoveBonus(CharacterStats stat, PassiveBonusInfo bonusInfo)
    {
        if (!passiveBonuses.ContainsKey(stat))
        {
            Debug.LogWarning("Trying to remove bonus that doesnt exist");
            return;
        }
        passiveBonuses[stat].Remove(bonusInfo);
        OnBonusLost?.Invoke(stat, bonusInfo);
    }

    /// <summary>
    /// Modifies a value by all character bonuses
    /// </summary>
    /// <param name="stat">Stat to modify</param>
    /// <param name="baseValue">value to change</param>
    public void ApplyAllBonuses(CharacterStats stat,ref float baseValue)
    {
        if (!passiveBonuses.ContainsKey(stat))
            return;

        foreach (var bonus in passiveBonuses[stat])
        {
            ApplyBonusToValue(ref baseValue, bonus.increaseType,bonus.value);
        }
    }
    /// <summary>
    /// Modifies value. Can be used for all bonuses. Maybe move it to it's own class?
    /// </summary>
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
                baseValue *= 1 + increaseValue * 0.01f;
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


