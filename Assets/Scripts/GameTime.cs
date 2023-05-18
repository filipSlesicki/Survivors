using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public static int seconds { get; private set; }
    public static int minutes { get; private set; }
    public static int totalTimeInSeconds { get; private set; }

    private void Start()
    {
        StartCoroutine(GameTimeCor());
    }

    IEnumerator GameTimeCor()
    {
        totalTimeInSeconds = 0;
        seconds = 0;
        minutes = 0;
        text.text = "0";
        while (true)
        {
            yield return new WaitForSeconds(1);
            totalTimeInSeconds++;
            seconds++;
            if(seconds>60)
            {
                seconds = 0;
                minutes++;
            }

            text.text = GetTimeString();
        }
    }

    public static string GetTimeString()
    {
        if (minutes > 0)
        {
           return minutes.ToString() + ":" + seconds.ToString();
        }
        else
        {
            return seconds.ToString();
        }
    }
}

