using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponBonus
{
    public WeaponStat stat;
    public IncreaseType increaseType;
    public float increaseValue;

    public void BonusDescrition(System.Text.StringBuilder sb)
    {
        sb.Append(stat.ToString());
        sb.Append(" ");
        switch (increaseType)
        {
            case IncreaseType.Additive:
                if (increaseValue >= 0)
                    sb.Append("+ ");
                sb.Append(increaseValue);
                break;
            case IncreaseType.Multiplicative:
                if(increaseValue >= 1)
                {
                    sb.Append("* ");
                    sb.Append(increaseValue);
                }
                else
                {
                    sb.Append("/ ");
                    sb.Append(1 / increaseValue);
                }
                break;
            case IncreaseType.AddPercent:
                if (increaseValue >= 0)
                    sb.Append("+ ");
                sb.Append(increaseValue);
                sb.Append("%");
                break;
            case IncreaseType.Set:
                sb.Append("= ");
                sb.Append(increaseType);
                break;
            default:
                break;
        }
        sb.Append("\n");
      
    }


}

[System.Serializable]
public class WeaponBonusPerLevel
{
    public WeaponBonus[] bonuses;
}

