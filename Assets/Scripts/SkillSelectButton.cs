using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectButton : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI descriptionText;

    SkillData skillData;
    public void Setup(SkillData data)
    {
        icon.sprite = data.Icon;

        skillName.text = data.name + " lv "+(PlayerSkills.Instance.GetSkillLevel(data) +1).ToString();
        descriptionText.text = PlayerSkills.Instance.GetSkillDescriptionText(data);
        this.skillData = data;
    }

    public void SelectSkill()
    {
        PlayerSkills.Instance.AddSkill(skillData);
        
    }
}
