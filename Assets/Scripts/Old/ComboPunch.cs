using UnityEngine;
using UnityEngine.UI;

public class ComboPunch : MonoBehaviour
{
    public Animator femanimator;  // Reference to the animator
    public Button punchButton;  // Reference to the punch button

    private float doubleTapTime = 0.5f; // Time window for detecting double tap
    private float lastPunchTime = -1f; // To keep track of the last punch time

    public void Start()
    {
        // Ensure animator is assigned in the inspector
        if (femanimator == null)
        {
            femanimator = GetComponent<Animator>();
        }

        // Ensure punchButton is assigned in the inspector
        if (punchButton != null)
        {
            punchButton.onClick.AddListener(OnPunchButtonClick);
        }
        else
        {
            Debug.LogError("Punch button not assigned in the inspector");
        }
    }

    private void OnPunchButtonClick()
    {
        float currentTime = Time.time;

        if (currentTime - lastPunchTime < doubleTapTime)
        {
            // Double tap detected
            TriggerAnimation("femcombokick");
        }
        else
        {
            // Single tap
            TriggerAnimation("femcombopunch");
        }

        lastPunchTime = currentTime;
    }

    private void TriggerAnimation(string animationName)
    {
        // Trigger the desired animation
        femanimator.SetTrigger(animationName);
    }
}
