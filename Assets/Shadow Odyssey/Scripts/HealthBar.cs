using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the slider UI component
    public Gradient gabriellaHealthGradient; // Color gradient for Gabriella (green to red)
    public Gradient marcusHealthGradient; // Color gradient for Marcus (dark violet to red)
    public Image fill; // Image component to show gradient color

    // This should be set to the character using the health bar
    public string characterName; // "Gabriella" or "Marcus"
    private const float lowHealthThreshold = 0.3f; // 30% health threshold for low health

    // Sets the maximum health value for the slider and updates the color to full health
    public void SetMaxHealth(int health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
            UpdateHealthColor(health); // Initialize health color on max health
        }
        else
        {
            Debug.LogError("Slider component not assigned in HealthBar script.");
        }
    }

    // Updates the slider value and color based on the current health
    public void SetHealth(int health)
    {
        if (slider != null)
        {
            slider.value = health;
            UpdateHealthColor(health); // Update the color based on current health
        }
        else
        {
            Debug.LogError("Slider component not assigned in HealthBar script.");
        }
    }

    private void UpdateHealthColor(int health)
    {
        if (fill != null)
        {
            // Determine the gradient to use based on the character
            Gradient currentGradient = characterName == "Gabriella" ? gabriellaHealthGradient : marcusHealthGradient;

            // Calculate normalized health value
            float normalizedValue = slider.value / slider.maxValue;

            // Change color to red if health is below the threshold
            if (normalizedValue < lowHealthThreshold)
            {
                fill.color = Color.red; // Low health color
            }
            else
            {
                fill.color = currentGradient.Evaluate(normalizedValue); // Use the gradient for full health
            }
        }
        else
        {
            Debug.LogError("Fill Image component not assigned in HealthBar script.");
        }
    }

    // Decrease health over time
    public void DecreaseHealthOverTime(int damagePerSecond, float duration)
    {
        StartCoroutine(DecreaseHealthCoroutine(damagePerSecond, duration));
    }

    private System.Collections.IEnumerator DecreaseHealthCoroutine(int damagePerSecond, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            SetHealth((int)(slider.value - damagePerSecond * Time.deltaTime));
            yield return null; // Wait for the next frame
            timeElapsed += Time.deltaTime;
        }

        // Ensure health does not go below 0
        SetHealth(0);
    }
}
