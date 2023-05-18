using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bonuses
{
    Dictionary<PlayerStats,List<PassiveBonusInfo>> passiveBonuses = new Dictionary<PlayerStats, List<PassiveBonusInfo>>();
    public event Action<PlayerStats, PassiveBonusInfo> OnBonusGained;
    public event Action<PlayerStats, PassiveBonusInfo> OnBonusLost;

    //public Bonuses()
    //{
    //    foreach (PlayerStats stat in Enum.GetValues(typeof( PlayerStats)))
    //    {
    //        passiveBonuses.Add(stat, new List<PassiveBonusInfo>());
    //    }
    //}

    public void AddBonus(PlayerStats stat, PassiveBonusInfo bonusInfo)
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

    public void ApplyAllBonuses(PlayerStats stat,ref float baseValue)
    {
        if (!passiveBonuses.ContainsKey(stat))
            return;

        foreach (var bonus in passiveBonuses[stat])
        {
            ApplyBonusToValue(ref baseValue, bonus);
        }
    }

    public static void ApplyBonusToValue(ref float baseValue, PassiveBonusInfo bonusInfo)
    {
        switch (bonusInfo.increaseType)
        {
            case IncreaseType.Additive:
                baseValue += bonusInfo.value;
                break;
            case IncreaseType.Multiplicative:
                baseValue *= bonusInfo.value;
                break;
            case IncreaseType.AddPercent:
                baseValue *= 1 + bonusInfo.value / 100;
                break;
            default:
                break;
        }
    }

    void RemoveBonus(PlayerStats stat, PassiveBonusInfo bonusInfo)
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


