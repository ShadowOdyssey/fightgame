using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the slider UI component
    public Gradient colorGradient; // Color gradient for current Life Bar
    public Image sliderFill; // Image component to show gradient color

    private float normalizedValue = 0f;

    public void Update()
    {
        Debug.Log("Slider " + gameObject.name + " value is: " + slider.value);
    }

    // Sets the maximum health value for the slider and updates the color to full health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        UpdateHealthColor(health); // Initialize health color on max health
    }

    // Updates the slider value and color based on the current health
    public void SetHealth(int health)
    {
        slider.value = health;
        UpdateHealthColor(health); // Update the color based on current health
    }

    private void UpdateHealthColor(int health)
    {
        // Calculate normalized health value
        normalizedValue = slider.value / slider.maxValue;
        sliderFill.color = colorGradient.Evaluate(normalizedValue);
    }
}
