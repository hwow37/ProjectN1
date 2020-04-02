using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxJump(float jump)
    {
        slider.maxValue = jump;
        slider.value = jump;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetJump(float jump)
    {
        slider.value = jump;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
