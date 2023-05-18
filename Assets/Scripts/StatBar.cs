using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] bool startFilled;
    private void Start()
    {
        if (startFilled)
        {
            bar.fillAmount = 1;
        }
        else
            bar.fillAmount = 0;
    }
    public void SetValue(float current,float max)
    {
        bar.fillAmount = (float)current/max;
    }
}
