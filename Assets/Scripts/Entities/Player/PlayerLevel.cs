using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] ExpValues expValues;
    public FloatEvent onExpGainedEvent;
    public IntEvent OnLevelUpEvent;

    public static PlayerLevel Instance;
    int exp;
    int level;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.altKey.wasPressedThisFrame)
        {
            LevelUp();
        }
    }
    public void AddExp(int amount)
    {
        exp += amount;
        if (exp >= expValues.expPerLevel[level])
        {
            LevelUp();
        }
        onExpGainedEvent.Invoke(exp/ expValues.expPerLevel[level]);
    }

    void LevelUp()
    {
        exp -= expValues.expPerLevel[level];
        level++;
        OnLevelUpEvent.Invoke(level);
    }
}
