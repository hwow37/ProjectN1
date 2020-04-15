using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // for setting Energy
    [SerializeField] private GameObject textDarkEnergy;
    [SerializeField] private GameObject textLightEnergy;
    private float animationTime = 1.5f;
    private float desiredDarkEnergy;
    private float initialDarkEnergy;
    private float currentDarkEnergy;

    private void Start()
    {
        SetMaxHealth(4);
        SetEnergy(10000);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackedHealth();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            AddEnergy(-100);
        }

        SlidingNumber();
    }

    // HealthBar
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void AttackedHealth()
    {
        slider.value = slider.value - 1;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


    // Sliding Number
    public void SetEnergy(float value)
    {
        initialDarkEnergy = currentDarkEnergy;
        desiredDarkEnergy = value;
    }
    public void AddEnergy(float value)
    {
        initialDarkEnergy = currentDarkEnergy;
        desiredDarkEnergy += value;
    }
    private void SlidingNumber()
    {
        if (currentDarkEnergy != desiredDarkEnergy)
        {
            if (initialDarkEnergy < desiredDarkEnergy)
            {
                currentDarkEnergy += (animationTime * Time.deltaTime) * (desiredDarkEnergy - initialDarkEnergy);
                if (currentDarkEnergy >= desiredDarkEnergy)
                {
                    currentDarkEnergy = desiredDarkEnergy;
                }
            }
            else
            {
                currentDarkEnergy -= (animationTime * Time.deltaTime) * (initialDarkEnergy - desiredDarkEnergy);
                if (currentDarkEnergy <= desiredDarkEnergy)
                {
                    currentDarkEnergy = desiredDarkEnergy;
                }
            }
            // for changing int value
            textDarkEnergy.GetComponent<TextMeshProUGUI>().text = currentDarkEnergy.ToString("0");
        }
    }
}
