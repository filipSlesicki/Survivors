using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillData : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    public virtual int MaxLevel { get; }

}
