using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Color loadColor;
    public Color fullColor;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateMeter(float percent)
    {
        SetSliderValue(percent);

        if (slider.value < slider.maxValue)
        {
            SetFillColor(loadColor);
        }
        else
        {
            SetFillColor(fullColor);
        }
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetFillColor(Color color)
    {
        fill.color = color;
    }
}
