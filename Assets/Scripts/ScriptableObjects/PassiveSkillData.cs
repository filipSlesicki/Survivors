using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Skill", menuName = "Passive Skills")]
public class PassiveSkillData : SkillData
{
    [field: SerializeField] public int _MaxLevel { get; private set; }
    [field: SerializeField] public CharacterStats Stat { get; private set; }
    [field: SerializeField] public IncreaseType IncreaseType { get; private set; }
    [field: SerializeField] public float IncreasePerLevel { get; private set; }
    public override int MaxLevel => _MaxLevel;

    public string GetDescription()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(Stat.ToString());
        sb.Append(" ");
        switch (IncreaseType)
        {
            case IncreaseType.Additive:

                sb.Append("+ " + IncreasePerLevel);
                break;
            case IncreaseType.Multiplicative:
                sb.Append("* " + IncreasePerLevel);
                break;
            case IncreaseType.AddPercent:
                if(IncreasePerLevel>0)
                {
                    sb.Append("+");
                }
                sb.Append(IncreasePerLevel +"%");
                break;
            default:
                break;
        }
        return sb.ToString();
    }
}


