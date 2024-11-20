using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;  // Lowered speed to slow down movement
    public Joystick joystick;
    public Animator femanimator;  // Reference to the animator
    public Rigidbody rb;  // Making this public to ensure it's properly assigned
    public Button punchButton;  // Reference to the punch button

    private float doubleTapTime = 0.5f; // Time window for detecting double tap
    private float lastPunchTime = -1f; // To keep track of the last punch time

    public void Start()
    {
        // Ensure the Rigidbody is assigned either via Inspector or code
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

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

    private void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 move = direction * speed * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f);

            // Check for movement direction and trigger corresponding animations
            if (horizontal > 0) // Moving right
            {
                TriggerAnimation("femforward");
            }
            else if (horizontal < 0) // Moving left
            {
                TriggerAnimation("fembackward");
            }
            else if (vertical > 0) // Moving up
            {
                TriggerAnimation("fempunchelbow");
            }
            else if (vertical < 0) // Moving down (if any specific animation for moving down)
            {
                TriggerAnimation("femidle"); // Assuming down should also lead to idle
            }
        }
        else
        {
            // If not moving, set idle animation
            TriggerAnimation("femidle");
        }
    }

    private void TriggerAnimation(string animationName)
    {
        // Reset all relevant animator parameters
        femanimator.SetBool("femforward", false);
        femanimator.SetBool("fembackward", false);
        femanimator.SetBool("femidle", false);

        // Set the desired animation
        if (animationName == "femforward" || animationName == "fembackward" || animationName == "femidle")
        {
            femanimator.SetBool(animationName, true);
        }
        else
        {
            femanimator.SetTrigger(animationName);
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
}
