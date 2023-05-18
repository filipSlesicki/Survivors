using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillSelectScreen : MonoBehaviour
{
    public List<SkillData> allSkills;
    [SerializeField] GameObject skillSelectScreen;

    [SerializeField] SkillSelectButton[] skillSelectButtons;

    //private void Start()
    //{
    //    ShowSkillsToSelect(0);
    //}

    private void OnEnable()
    {
        PlayerLevel.OnLevelUp += ShowSkillsToSelect;
        PlayerSkills.OnSkillSelected += OnSkillSelected;
    }

    private void OnDisable()
    {
        PlayerLevel.OnLevelUp -= ShowSkillsToSelect;
        PlayerSkills.OnSkillSelected -= OnSkillSelected;
    }

    public void ShowSkillsToSelect(int level)
    {
        Time.timeScale = 0;
        skillSelectScreen.SetActive(true);
        int skillsToShow = skillSelectButtons.Length;
        SkillData[] skillsToSelect = allSkills.OrderBy(x => Random.value).Take(skillsToShow).ToArray();
        for (int i = 0; i < skillsToSelect.Length; i++)
        {
            skillSelectButtons[i].Setup(skillsToSelect[i]);
        }
    }

    public void OnSkillSelected(SkillData skill, int level)
    {
        skillSelectScreen.SetActive(false);
        Time.timeScale = 1;
        if (level >= skill.MaxLevel)
        {
            allSkills.Remove(skill);
            DisableExtraButtons();
        }
    }
    void DisableExtraButtons()
    {
        if (allSkills.Count == 2)
        {
            skillSelectButtons[2].gameObject.SetActive(false);
        }
        else if (allSkills.Count == 1)
        {
            skillSelectButtons[1].gameObject.SetActive(false);
        }
        else if (allSkills.Count == 0)
        {
            skillSelectButtons[0].gameObject.SetActive(false);
        }
    }
}
