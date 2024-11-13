using UnityEngine;
using UnityEngine.UI;

public class AdjustJoystickSizeAndImage : MonoBehaviour
{
    public RectTransform joystickBackground;
    public RectTransform joystickKnob;
    public Vector2 newSizeBackground;
    public Vector2 newSizeKnob;
    public Image joystickBackgroundImage;
    public Sprite newSprite;

    // References to the buttons
    public Button comboPunchButton;
    public Button comboKickButton;
    public Button punchElbowButton;

    // Reference to the Animator
    public Animator femAnimator;

    void Start()
    {
        // Adjust the size of the joystick background
        if (joystickBackground != null)
        {
            joystickBackground.sizeDelta = newSizeBackground;
        }

        // Adjust the size of the joystick knob
        if (joystickKnob != null)
        {
            joystickKnob.sizeDelta = newSizeKnob;
        }

        // Change the image of the joystick background
        if (joystickBackgroundImage != null && newSprite != null)
        {
            joystickBackgroundImage.sprite = newSprite;
        }

        // Ensure buttons are assigned and add listeners to them
        if (comboPunchButton != null)
        {
            comboPunchButton.onClick.AddListener(() => PlayAnimation("femcombopunch"));
        }

        if (comboKickButton != null)
        {
            comboKickButton.onClick.AddListener(() => PlayAnimation("femcombokick"));
        }

        if (punchElbowButton != null)
        {
            punchElbowButton.onClick.AddListener(() => PlayAnimation("fempunchelbow"));
        }
    }

    void PlayAnimation(string triggerName)
    {
        if (femAnimator != null)
        {
            femAnimator.SetTrigger(triggerName);
        }
    }
}
